using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Linq;
using System.Security.Cryptography;
using System.Security.AccessControl;
using D3_Adventures.Memory_Handling;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace D3_Adventures.Injector
{
    public sealed class ASMExecutor : IDisposable
    {
        // Fields
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        [CompilerGenerated]
        private bool bool_3;
        private readonly byte[] byte_0;
        private byte[] byte_1;
        private readonly EventWaitHandle eventWaitHandle_0;
        private readonly string eventWaitHandle_0_Name;
        private readonly EventWaitHandle eventWaitHandle_1;
        private readonly string eventWaitHandle_1_Name;
        private readonly EventWaitHandle eventWaitHandle_2;
        private readonly string eventWaitHandle_2_Name;
        private IntPtr intptr_1;
        private IntPtr intptr_2;
        private IntPtr intptr_3;
        private IntPtr intptr_4;
        private IntPtr intptr_5;
        private IntPtr intptr_6;
        private IntPtr intptr_7;
        private IntPtr pntCreateEventA;
        private readonly IntPtr pntEndScene;
        private IntPtr pntResetEvent;
        private IntPtr pntSetEvent;
        private IntPtr pntWaitForSingleObject;
        private readonly object privateLock;
        public object publicLock;
        private readonly Random random_0;

        // Methods
        public ASMExecutor(MemoryManager memory, IntPtr endscene)
        {
            bool flag;
            this.privateLock = new object();
            this.byte_0 = new byte[0x1000];
            this.publicLock = new object();
            if (!((memory != null) && memory.IsProcessOpen))
            {
                throw new ArgumentNullException("memory", "Memory object passed to Executor constructor was invalid");
            }
            if (endscene == IntPtr.Zero)
            {
                throw new ArgumentNullException("endscene", "Endscene pointer passed to Executor constructor was invalid");
            }
            this.Memory = memory;
            this.Memory.Asm.SetMemorySize(0x10000);
            this.pntEndScene = endscene;
            byte[] data = new byte[4];
            new RNGCryptoServiceProvider().GetNonZeroBytes(data);
            this.random_0 = new Random(BitConverter.ToInt32(data, 0));
            string identity = Environment.UserDomainName + @"\" + Environment.UserName;
            EventWaitHandleSecurity eventSecurity = new EventWaitHandleSecurity();
            EventWaitHandleAccessRule rule = new EventWaitHandleAccessRule(identity, EventWaitHandleRights.FullControl, AccessControlType.Allow);
            eventSecurity.AddAccessRule(rule);
            this.eventWaitHandle_0_Name = @"Global\" + this.RandomString(0x10);
            this.eventWaitHandle_0 = new EventWaitHandle(false, EventResetMode.AutoReset, this.eventWaitHandle_0_Name, out flag, eventSecurity);
            if (!flag)
            {
                throw new Exception("You should never see this message but the event was opened instead of created! That's bad!");
            }
            this.eventWaitHandle_1_Name = @"Global\" + this.RandomString(0x10);
            this.eventWaitHandle_1 = new EventWaitHandle(false, EventResetMode.AutoReset, this.eventWaitHandle_1_Name, out flag, eventSecurity);
            if (!flag)
            {
                throw new Exception("You should never see this message but the event was opened instead of created! That's bad!");
            }
            this.eventWaitHandle_2_Name = @"Global\" + this.RandomString(0x10);
            this.eventWaitHandle_2 = new EventWaitHandle(false, EventResetMode.AutoReset, this.eventWaitHandle_2_Name, out flag, eventSecurity);
            if (!flag)
            {
                throw new Exception("You should never see this message but the event was opened instead of created! That's bad!");
            }
            this.Initialize();
            this.Clear();
            this.IsInitialized = true;
        }
        public IntPtr CallFunction(uint adress, CallingConvention callConv, params object[] args)
        {
            lock (publicLock)
            {
                Clear();
                foreach (string str in MakeFunctionCall(adress, callConv, args))
                {
                    AddLineSecured(str);
                }
                ExecuteBuffer(null);

                return ReturnPointer;
            }
        }
        private IEnumerable<string> MakeFunctionCall(uint address, CallingConvention callingConvention, params object[] args)
        {
            int tmp = 0;
            long l0;
            ulong ul0;
            if (callingConvention == CallingConvention.ThisCall)
            {
                if (((args.Length <= 0) || (args[0].GetType() != typeof(uint))) || (((uint)args[0]) == 0))
                {
                    throw new ArgumentException("First argument when specifying ThisCall must be a nonnull uint");
                }
                yield return ("mov ecx, " + ((uint)args[0]));
                tmp = 1;
            }
            uint tmp2 = 0;
            int tmp3 = args.Length - 1;

            goto Label_0195;


        Label_0187:
            tmp3--;

        Label_0195:
            if (tmp3 < tmp)
            {
                yield return ("call " + address);
                if (callingConvention == CallingConvention.Cdecl)
                {
                    yield return ("add esp, " + tmp2);
                }
            }
            else
            {
                switch (Type.GetTypeCode(args[tmp3].GetType()))
                {
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                        yield return ("push " + args[tmp3]);
                        tmp2 += 4;
                        goto Label_0187;

                    case TypeCode.Int64:
                        l0 = (long)args[tmp3];
                        yield return ("push " + (l0 >> 0x20));
                        yield return ("push " + ((uint)l0));
                        tmp2 += 8;
                        goto Label_0187;

                    case TypeCode.UInt64:
                        ul0 = (ulong)args[tmp3];
                        yield return ("push " + (ul0 >> 0x20));
                        yield return ("push " + ((uint)ul0));
                        tmp2 += 8;
                        goto Label_0187;

                    default:
                        throw new ArgumentException(string.Format("Argument of type {0} can not be pushed onto the argument stack", args[tmp3].GetType().Name));
                }
            }
            yield return "retn";
        }
        public void AddLine(string szLine)
        {
            this.Memory.Asm.AddLine(szLine);
        }

        public void AddLine(string szFormatString, params object[] args)
        {
            this.Memory.Asm.AddLine(szFormatString, args);
        }

        public void AddLineSecured(string szLine)
        {
            this.AddLineSecured(szLine, new object[0]);
        }

        public void AddLineSecured(int numRandomLines, string szLine)
        {
            this.AddLineSecured(numRandomLines, szLine, new object[0]);
        }

        public void AddLineSecured(string szFormatString, params object[] args)
        {
            this.AddLineSecured(this.random_0.Next(2, 5), szFormatString, args);
        }

        public void AddLineSecured(int numRandomLines, string szFormatString, params object[] args)
        {
            if (numRandomLines <= 1)
            {
                numRandomLines = 2;
            }
            int num = (int)(((float)numRandomLines) / 2f);
            int num2 = num + (numRandomLines % 2);
            this.WriteBogusLines(num);
            this.AddLine(szFormatString, args);
            this.WriteBogusLines(num2);
        }

        public void Clear()
        {
            this.Memory.Asm.Clear();
        }

        public void Dispose()
        {
            if (!this.bool_2)
            {
                this.IsInitialized = false;
                this.Memory.WriteMemory(this.pntEndScene, this.byte_1.Length, ref this.byte_1);
                if (this.Memory != null)
                {
                    this.Memory.FreeMemory(this.intptr_1);
                    this.Memory.FreeMemory(this.intptr_2);
                    this.Memory.FreeMemory(this.intptr_3);
                }
                this.eventWaitHandle_0.Close();
                this.eventWaitHandle_1.Close();
                this.eventWaitHandle_2.Close();
            }
            this.bool_2 = true;
        }

        public void ExecuteBuffer(string debugMessage = null)
    {
        if (this.IsOpen && this.IsInitialized)
        {
            lock (this.privateLock)
            {
                if (this.bool_0)
                {
                    if (this.bool_1)
                    {
                        this.Memory.Asm.Inject((uint) this.intptr_2);
                        this.eventWaitHandle_1.Reset();
                        this.eventWaitHandle_0.Set();
                        if (!this.eventWaitHandle_2.WaitOne(0x2710, false))
                        {
                            throw new Exception("Process must have frozen or gotten out of sync: InjectionFinishedEvent_was_never_fired");
                        }
                        this.bool_1 = false;
                        return;
                    }
                    this.Memory.Asm.Inject((uint) this.intptr_2);
                    this.eventWaitHandle_0.Set();
                    this.eventWaitHandle_1.Set();
                    if (this.eventWaitHandle_2.WaitOne(0x2710, false))
                    {
                    }
                    throw new Exception("Process must have frozen or gotten out of sync: InjectionFinishedEvent was never fired");
                }
                this.Memory.Asm.Inject((uint) this.intptr_2);
                this.eventWaitHandle_0.Set();
                if (!this.eventWaitHandle_2.WaitOne(0x2710, false))
                {
                    throw new Exception("Process must have frozen or gotten out of sync: InjectionFinishedEvent was never fired");
                }
                this.eventWaitHandle_0.Reset();
                this.eventWaitHandle_1.Set();
            }
        }
        throw new Exception("Cannot execute code while process is not opened and/or Executor is not initialized!");
    }

        ~ASMExecutor()
        {
            this.Dispose();
        }

        private string GetRandomRegister()
        {
            switch (this.random_0.Next(1, 7))
            {
                case 1:
                    return "eax";

                case 2:
                    return "ebx";

                case 3:
                    return "ecx";

                case 4:
                    return "edx";

                case 5:
                    return "edi";

                case 6:
                    return "esi";
            }
            return "eax";
        }

        private string GetRandomShortRegister()
        {
            switch (this.random_0.Next(1, 7))
            {
                case 1:
                    return "ax";

                case 2:
                    return "bx";

                case 3:
                    return "cx";

                case 4:
                    return "dx";

                case 5:
                    return "di";

                case 6:
                    return "si";
            }
            return "ax";
        }

        private void Initialize()
        {
            if (!this.IsOpen)
            {
                throw new Exception("Process is not open for memory manipulation");
            }
            int length = this.byte_0.Length;
            if (this.Memory.GetProcessModule("d3d9.dll") == null)
            {
                throw new Exception("Executor can only be used on processes that use DirectX9");
            }
            IntPtr moduleHandleW = Imports.GetModuleHandleW("kernel32.dll");
            if (moduleHandleW == IntPtr.Zero)
            {
                throw new Exception("Could_not get handle to kernel32.dll");
            }
            this.pntWaitForSingleObject = Imports.GetProcAddress(moduleHandleW, "WaitForSingleObject");
            if (this.pntWaitForSingleObject == IntPtr.Zero)
            {
                throw new Exception("Could not get proc address of WaitForSingleObject");
            }
            this.pntCreateEventA = Imports.GetProcAddress(moduleHandleW, "CreateEventA");
            if (this.pntCreateEventA == IntPtr.Zero)
            {
                throw new Exception("Could not get proc address of CreateEventA");
            }
            this.pntResetEvent = Imports.GetProcAddress(moduleHandleW, "ResetEvent");
            if (this.pntResetEvent == IntPtr.Zero)
            {
                throw new Exception("Could not get proc address of ResetEvent");
            }
            this.pntSetEvent = Imports.GetProcAddress(moduleHandleW, "SetEvent");
            if (this.pntSetEvent == IntPtr.Zero)
            {
                throw new Exception("Could not get proc address of SetEvent");
            }
            IntPtr eventStub = this.Memory.AllocateMemory(length, Imports.AllocationType.Commit, Imports.MemoryProtection.ExecuteRead);
            if (eventStub == IntPtr.Zero)
            {
                throw new Exception("Could not allocate memory in target process for event stub");
            }
            IntPtr address = this.Memory.AllocateMemory(length, Imports.AllocationType.Commit, Imports.MemoryProtection.ReadWrite);
            if (address == IntPtr.Zero)
            {
                throw new Exception("Could not allocate memory in target process for event names");
            }
            IntPtr ptr4 = (address + this.eventWaitHandle_0_Name.Length) + 4;
            IntPtr ptr5 = (ptr4 + this.eventWaitHandle_1_Name.Length) + 4;
            if (!((this.Memory.WriteText(address, this.eventWaitHandle_0_Name, Encoding.UTF8) && this.Memory.WriteText(ptr4, this.eventWaitHandle_1_Name, Encoding.UTF8)) && this.Memory.WriteText(ptr5, this.eventWaitHandle_2_Name, Encoding.UTF8)))
            {
                throw new Exception("Could not write event names to memory!");
            }
            this.intptr_1 = this.Memory.AllocateMemory(length, Imports.AllocationType.Commit, Imports.MemoryProtection.ExecuteRead);
            if (this.intptr_1 == IntPtr.Zero)
            {
                throw new Exception("Could not allocate memory in target process for detour");
            }
            this.intptr_2 = this.Memory.AllocateMemory(length, Imports.AllocationType.Commit, Imports.MemoryProtection.NoCacheModifierflag | Imports.MemoryProtection.ExecuteReadWrite);
            if (this.intptr_2 == IntPtr.Zero)
            {
                throw new Exception("Could not allocate memory in target process for injected code");
            }
            this.intptr_3 = this.Memory.AllocateMemory(length, Imports.AllocationType.Commit, Imports.MemoryProtection.ReadWrite);
            if (this.intptr_3 == IntPtr.Zero)
            {
                throw new Exception("Could not allocate memory in target process for injected data");
            }
            this.intptr_5 = this.intptr_3;
            this.intptr_6 = this.intptr_3 + 4;
            this.intptr_7 = this.intptr_3 + 8;
            this.intptr_4 = this.intptr_3 + 12;
            if (!this.method_18(eventStub, address, ptr4, ptr5))
            {
                throw new Exception("Could not successfully set up_synchronization_events!");
            }
            this.InjectStub();
            this.Memory.FreeMemory(eventStub);
            this.Memory.FreeMemory(address);
        }

        private void InjectStub()
    {
        this.Clear();
        this.AddLineSecured("pushad");
        this.AddLineSecured("@CheckInjection:");
        this.AddLineSecured("mov eax, [{0}]", new object[] { this.intptr_5 });
        this.AddLineSecured("push 0");
        this.AddLineSecured("push eax");
        this.AddLineSecured("call {0}", new object[] { this.pntWaitForSingleObject });
        this.AddLineSecured("test eax, eax");
        this.AddLineSecured("jnz @NoInjection");
        this.AddLineSecured("call {0}", new object[] { this.intptr_2 });
        this.AddLineSecured("mov [{0}], eax", new object[] { this.intptr_4 });
        this.AddLineSecured("mov eax, [{0}]", new object[] { this.intptr_7 });
        this.AddLineSecured("push eax");
        this.AddLineSecured("call {0}", new object[] { this.pntSetEvent });
        this.AddLineSecured("mov eax, [{0}]", new object[] { this.intptr_6 });
        this.AddLineSecured("push 1000");
        this.AddLineSecured("push eax");
        this.AddLineSecured("call {0}", new object[] { this.pntWaitForSingleObject });
        this.AddLineSecured("test eax, eax");
        this.AddLineSecured("jz @CheckInjection");
        this.AddLine("@NoInjection:");
        this.AddLineSecured("popad");
        byte[] first = this.Memory.unsafeRead(this.pntEndScene, 6, false);
        this.byte_1 = first;
        switch ((first.SequenceEqual<byte>(new byte[] { 0x55, 0x8b, 0xec, 0x8b, 0x45, 8 }) ? Enum4.const_1 : Enum4.const_0))
        {
            case Enum4.const_0:
                this.AddLineSecured("mov edi, edi");
                this.AddLineSecured("push ebp");
                this.AddLineSecured("mov ebp, esp");
                this.AddLineSecured("jmp {0}", new object[] { this.pntEndScene + 5 });
                break;

            case Enum4.const_1:
                this.AddLineSecured("push ebp");
                this.AddLineSecured("mov ebp, esp");
                this.AddLineSecured("mov eax, [ebp+8]");
                this.AddLineSecured("jmp {0}", new object[] { this.pntEndScene + 6 });
                break;
        }
        if (!this.Memory.Asm.Inject((uint) this.intptr_1))
        {
            throw new Exception("Could not assemble and inject trampoline");
        }
        this.Clear();
        this.AddLine("jmp {0}", new object[] { this.intptr_1 });
        if (!this.Memory.Asm.Inject((uint) this.pntEndScene))
        {
            throw new Exception("Could not assemble and inject detour");
        }
    }

        public void method_0()
        {
            lock (this.privateLock)
            {
                if (this.bool_0)
                {
                    this.method_1();
                }
                this.bool_0 = true;
                this.bool_1 = true;
            }
        }

        public void method_1()
        {
            lock (this.privateLock)
            {
                this.bool_0 = false;
                this.bool_1 = false;
                this.eventWaitHandle_0.Reset();
                this.eventWaitHandle_1.Set();
            }
        }

        private bool method_18(IntPtr eventStub, IntPtr injectionWaitingNamePtr, IntPtr injectionContinueNamePtr, IntPtr injectionFinishedNamePtr)
    {
        this.Clear();
        this.AddLineSecured("push {0}", new object[] { injectionWaitingNamePtr });
        this.AddLineSecured("push 0");
        this.AddLineSecured("push 0");
        this.AddLineSecured("push 0");
        this.AddLineSecured("call {0}", new object[] { this.pntCreateEventA });
        this.AddLineSecured("test eax, eax");
        this.AddLineSecured("jz @ReturnFalse");
        this.AddLineSecured("mov [{0}], eax", new object[] { this.intptr_5 });
        this.AddLineSecured("push {0}", new object[] { injectionContinueNamePtr });
        this.AddLineSecured("push 0");
        this.AddLineSecured("push 0");
        this.AddLineSecured("push 0");
        this.AddLineSecured("call {0}", new object[] { this.pntCreateEventA });
        this.AddLineSecured("test eax, eax");
        this.AddLineSecured("jz @ReturnFalse");
        this.AddLineSecured("mov [{0}], eax", new object[] { this.intptr_6 });
        this.AddLineSecured("push {0}", new object[] { injectionFinishedNamePtr });
        this.AddLineSecured("push 0");
        this.AddLineSecured("push 0");
        this.AddLineSecured("push 0");
        this.AddLineSecured("call {0}", new object[] { this.pntCreateEventA });
        this.AddLineSecured("test eax, eax");
        this.AddLineSecured("jz @ReturnFalse");
        this.AddLineSecured("mov [{0}], eax", new object[] { this.intptr_7 });
        this.AddLine("mov eax, 1");
        this.AddLine("retn");
        this.AddLine("@ReturnFalse:");
        this.AddLine("xor eax, eax");
        this.AddLine("retn");
        return (this.Memory.Asm.InjectAndExecute((uint) eventStub) == 1);
    }

        private string RandomString(int length)
        {
            return this.RandomString(length, length);
        }

        private string RandomString(int minLength = 0x40, int maxLength = 0x40)
        {
            char[] chArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            StringBuilder builder = (minLength < maxLength) ? new StringBuilder(this.random_0.Next(minLength, maxLength)) : new StringBuilder(minLength);
            for (int i = 0; i < builder.Capacity; i++)
            {
                builder.Append(chArray[(int)(this.random_0.NextDouble() * (chArray.Length - 1))]);
            }
            return builder.ToString();
        }

        private void WriteBogusLines(int numRandomLines)
        {
            while (numRandomLines-- > 0)
            {
                int num;
                string randomRegister = this.GetRandomRegister();
                string randomShortRegister = this.GetRandomShortRegister();
                switch (this.random_0.Next(1, 6))
                {
                    case 1:
                        num = this.random_0.Next(1, 4);
                        goto Label_0066;

                    case 2:
                        {
                            this.AddLine("mov {0}, {0}", new object[] { randomRegister });
                            continue;
                        }
                    case 3:
                        {
                            this.AddLine("mov {0}, {0}", new object[] { randomShortRegister });
                            continue;
                        }
                    case 4:
                        {
                            this.AddLine("push {0}", new object[] { randomShortRegister });
                            this.AddLine("pop {0}", new object[] { randomShortRegister });
                            continue;
                        }
                    case 5:
                        {
                            this.AddLine("push {0}", new object[] { randomRegister });
                            this.AddLine("pop {0}", new object[] { randomRegister });
                            continue;
                        }
                    default:
                        {
                            continue;
                        }
                }
            Label_0054:
                this.AddLine("nop");
                num--;
            Label_0066:
                if (num > 0)
                {
                    goto Label_0054;
                }
            }
        }

        // Properties
        public IntPtr DataPointer
        {
            get
            {
                return this.intptr_3;
            }
        }

        public IntPtr InjectCodePointer
        {
            get
            {
                return this.intptr_2;
            }
        }

        public bool IsInitialized
        {
            [CompilerGenerated]
            get
            {
                return this.bool_3;
            }
            [CompilerGenerated]
            private set
            {
                this.bool_3 = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return (((this.Memory != null) && this.Memory.IsProcessOpen) && this.Memory.IsThreadOpen);
            }
        }

        public MemoryManager Memory { get; set; }

        public IntPtr ReturnPointer
        {
            get
            {
                return this.intptr_4;
            }
        }

        // Nested Types
        private enum Enum4
        {
            const_0,
            const_1
        }
    }

}
 
