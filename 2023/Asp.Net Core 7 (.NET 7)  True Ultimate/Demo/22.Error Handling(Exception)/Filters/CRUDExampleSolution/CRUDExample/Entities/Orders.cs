
using ServiceContracts.DTO;

namespace Entities
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; }

        public List<SellOrderResponse> SellOrders { get; set; }
    }
}
