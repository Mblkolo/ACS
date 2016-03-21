using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlSystem.Domain
{
    public class VaultAccess
    {
        public virtual int Id { get; set; }
        public virtual Vault Vault { get; set; }
        public virtual DateTime AccessTime { get; set; }
        public virtual bool IsSuccess { get; set; }
        public virtual string UserInfo { get; set; }
    }
}