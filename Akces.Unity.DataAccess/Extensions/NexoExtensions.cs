using System.Linq;
using System.Text;

namespace Akces.Unity.DataAccess
{
    internal static class NexoExtensions
    {
        internal static string WypiszBledy(this InsERT.Mox.ObiektyBiznesowe.IObiektBiznesowy obiektBiznesowy)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WypiszBledy((InsERT.Mox.BusinessObjects.IBusinessObject)obiektBiznesowy));
            var uow = ((InsERT.Mox.BusinessObjects.IGetUnitOfWork)obiektBiznesowy).UnitOfWork;
            foreach (var innyObiektBiznesowy in uow.Participants.OfType<InsERT.Mox.BusinessObjects.IBusinessObject>().Where(bo => bo != obiektBiznesowy))
            {
                sb.AppendLine(WypiszBledy(innyObiektBiznesowy));
            }
            return sb.ToString();
        }
        internal static string WypiszBledy(this InsERT.Mox.BusinessObjects.IBusinessObject obiektBiznesowy)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var encjaZBledami in obiektBiznesowy.InvalidData)
            {
                foreach (var bladNaCalejEncji in encjaZBledami.Errors)
                {
                    stringBuilder.AppendLine(bladNaCalejEncji.ToString());
                    stringBuilder.AppendLine(" na encjach:" + encjaZBledami.GetType().Name);
                    stringBuilder.AppendLine("");
                }
                foreach (var bladNaKonkretnychPolach in encjaZBledami.MemberErrors)
                {
                    stringBuilder.AppendLine(bladNaKonkretnychPolach.Key.ToString());
                    stringBuilder.AppendLine(" na polach:");
                    stringBuilder.AppendLine(string.Join(", ", bladNaKonkretnychPolach.Select(b => encjaZBledami.GetType().Name + "." + b)));
                    stringBuilder.AppendLine("");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
