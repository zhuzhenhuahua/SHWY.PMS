using SHWY.Model.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Lib.DB.Repositorys
{
    public class PartyRepository : BaseRepository
    {
        private static PartyRepository _partyRepository;
        private static readonly object locker = new object();
        private PartyRepository()
        {

        }
        public static PartyRepository CreateInstance()
        {
            if (_partyRepository == null)
            {
                lock (locker)
                {
                    if (_partyRepository == null)
                        _partyRepository = new PartyRepository();
                }
            }
            return _partyRepository;
        }
        public async Task<Tuple<int, List<Party>>> GetPartyListAsync(int pageIndex, int pageSize, string name)
        {
            int from = (pageIndex - 1) * pageSize;
            int total = await (from j in context.Partys
                               where string.IsNullOrEmpty(name) ? 1 == 1 : j.name.Contains(name)
                               select j).CountAsync();
            var list = await (from j in context.Partys
                              where string.IsNullOrEmpty(name) ? 1 == 1 : j.name.Contains(name)
                              orderby j.PartyID descending
                              select j).Skip(from).Take(pageSize).ToListAsync();
            return Tuple.Create(total, list);
        }
        public async Task<List<Party>> GetPartyListAsync()
        {
            var list = await (from j in context.Partys
                              select j).ToListAsync();
            return list;
        }
        public static ConcurrentDictionary<string, Party> _dicParty = new ConcurrentDictionary<string, Party>();
        public async Task<Party> GetPartyAsync(string partyID)
        {
            var model = await (from j in context.Partys
                               where j.PartyID == partyID
                               select j).FirstOrDefaultAsync();
            return model;

        }
        public async Task<Party> GetPartyDicAsync(string partyID)
        {
            try
            {
                if (!_dicParty.ContainsKey(partyID))
                {
                    var party = await (from j in context.Partys
                                       where j.PartyID == partyID
                                       select j).FirstOrDefaultAsync();
                    if (party != null)
                        _dicParty[partyID] = party;
                    else
                        return null;
                }
                return _dicParty[partyID];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region 增删改
        public async Task<bool> AddOrUpdateAsync(Party party)
        {
            try
            {
                var partyNew = await context.Partys.Where(p => p.PartyID == party.PartyID).FirstOrDefaultAsync();
                bool isNew = false;

                if (partyNew == null || string.IsNullOrEmpty(partyNew.PartyID))
                {
                    isNew = true;
                    partyNew = new Party() { PartyID = party.PartyID };
                }
                partyNew.name = party.name;
                partyNew.text = party.text;
                if (isNew)
                    context.Partys.Add(partyNew);
                return await context.SaveChangesAsync() == 1;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteParty(string partyID)
        {
            var party = await context.Partys.Where(p => p.PartyID == partyID).FirstOrDefaultAsync();
            if (party != null)
            {
                context.Partys.Remove(party);
                return await context.SaveChangesAsync() == 1;
            }
            return false;
        }
        #endregion
    }
}
