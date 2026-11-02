using System;

namespace GameOff2026.Common.Data.Entity
{
    public abstract class BaseStateEntity<T> : BaseEntity<T> where T : Enum
    {
    }
}