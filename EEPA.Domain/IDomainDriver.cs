﻿using System;

namespace EEPA.Domain
{
    public interface IDomainDriver
    {
        void AttachToSystem(string handleType);
        bool IsConnected { get; }
        event Func<dynamic, string> DomainEventHook;
    }
}