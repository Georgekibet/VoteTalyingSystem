using System;
using System.Collections.Generic;
using System.Text;
using vts.Shared.Entities.Master;

namespace vts.Shared.Repository
{
    public interface ISettingsRepository : IMasterRepository<Settings>
    {
        Settings GetByKey(SettingsKeys key);
    }
}
