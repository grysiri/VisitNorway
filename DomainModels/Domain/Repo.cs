using System.Linq;

namespace DomainModels.Domain
{
    public class Repo<T> where T : DomainBase
    {
        /*public Repo()
        {
            var itemList1 = All();
            var itemList2 = All2();

            var one = itemList1.Take(5);
            var three = itemList2.Take(5);
            foreach (var four in itemList2.Take(1))
            {

            }
            foreach (var two in one)
            {

            }
        }*/
        public T Get(int id)
        {
            return null;
        }

        public IQueryable<T> All() //Kjører spørringen før data legges i minnet, er å foretrekke
        {
            return null;
        }
        /*
        public IEnumerable<T> All2() //Legger alt i minnet og så kjører spørringen for å hente ut data
        {
            return null;
        }*/
    }
}
