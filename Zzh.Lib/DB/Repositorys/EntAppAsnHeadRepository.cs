using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzh.Model.DB;

namespace Zzh.Lib.DB.Repositorys
{
    public class EntAppAsnHeadRepository : BaseRepository
    {

        public EntAppAsnHeadRepository() : base("name=Default2Connection")
        {

        }   
        public List<EntAppAsnHead> GetList()
        {
            try
            {
                var obj = (from j in context.EntAppAsnHeads
                           select j).ToList();
                return obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
