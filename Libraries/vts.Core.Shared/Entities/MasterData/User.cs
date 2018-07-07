using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vts.Shared.Services;

namespace vts.Core.Shared.Entities.Master
{
    //TODO add user roles
    public class User : MasterEntity<UserRef>
    {
        public User() : base(default(Guid))
        {
        }

        public User(Guid id) : base(id)
        {
        }

        public User(Guid id, DateTime dateCreated, DateTime dateLastUpdated, EntityStatus status) : base(id, dateCreated, dateLastUpdated, status)
        {
        }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User type is required")]
        public UserType UserType { get; set; }

        [Required(ErrorMessage = "Mobile hone number is required")]
        public string Mobile { get; set; }

        public override UserRef GetMasterDataRef()
        {
            return new UserRef(this.Id, this.Username, this.UserType);
        }

        public override ValidationResultInfo Validate()
        {
            var validationInfo = this.BasicValidation();
            return validationInfo;
        }
    }

    [ComplexType]
    public class UserRef : MasterDataRef
    {
        public UserRef()
        {
        }

        public UserRef(Guid id, string username, UserType userType)
        {
            Id = id;
            Username = username;
            UserType = userType;
        }

        public string Username { get; set; }

        public UserType UserType { get; set; }

        public override MasterDataCollection MasterDataType
        {
            get { return MasterDataCollection.User; }
        }

        public override Tuple<bool, string> IsValid()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Username))
                message += "Invalid username";
            if (Id == Guid.Empty)
                message += "Invalid user id";
            if ((int)UserType == 0)
                message += "Invalid user type";
            return string.IsNullOrWhiteSpace(message) ? Tuple.Create(true, "") : Tuple.Create(false, message);
        }

        public static UserRef CreateEmpty()
        {
            return MasterDataRef.CreateEmpty<UserRef>();
        }

        protected override void CreatEmptyImpl()
        {
            this.Id = Guid.Empty;
            this.Username = "";
            this.UserType = UserType.None;
        }

        #region Equality

        protected bool Equals(UserRef other)
        {
            return Id.Equals(other.Id) && string.Equals(Username, other.Username) && UserType == other.UserType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)UserType;
                return hashCode;
            }
        }

        #endregion Equality
    }

    public enum UserType
    {
        None = 0,
        Admin = 1,
        Client = 2,
        Clerk = 3
    }
}