using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
    public interface IPathProvider
    {
        string MapPath(string path);
    }
}
