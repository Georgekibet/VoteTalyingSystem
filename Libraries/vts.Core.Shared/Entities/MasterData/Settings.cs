using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;

namespace vts.Shared.Entities.Master
{
    public class Settings : MasterEntity<SettingsRef>
    {
        public Settings() : base(default(Guid))
        {
        }
        public Settings(Guid id) : base(id)
        {
        }

        public Settings(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status) 
            : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        [Required(ErrorMessage = "Setting Key is required")]
        public SettingsKeys Key { get; set; }

        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }
        public override SettingsRef GetMasterDataRef()
        {
            return new SettingsRef(this.Id, this.Key);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class SettingsRef : MasterDataRef
    {
        public SettingsRef()
        {
        }

        public SettingsRef(Guid id, SettingsKeys key)
        {
            Id = id;
            Key = key;
        }
        public SettingsKeys Key { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.Settings; }
        }
        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if ((int)Key == 0)
                message += "Invalid setting";
            if (Id == Guid.Empty)
                message += "Invalid setting id";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static CountyRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<CountyRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Key = SettingsKeys.None;
        }

        #region Equality

        protected bool Equals(SettingsRef other)
        {
            return Id.Equals(other.Id) && Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SettingsRef) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode()*397) ^ (int) Key;
            }
        }

        #endregion
    }
}
