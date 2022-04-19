using Braintree;
using Microsoft.Extensions.Options;

namespace Rocky.Utils.BrainTree
{
    public class BrainTreeGate : IBrainTreeGate
    {
        private IBraintreeGateway BrainTreeGateWay { get; set; }

        public BrainTreeSettinngs Options { get; set; }

        public BrainTreeGate(IOptions<BrainTreeSettinngs> options)
        {
            Options = options.Value;
        }

        public IBraintreeGateway CreateGateWay()
        {
            return new BraintreeGateway(Options.Enviroment, Options.MerchantID, Options.PublicKey, Options.PrivateKey);
        }

        public IBraintreeGateway GetGateWay()
        {
            if (BrainTreeGateWay != null)
            {
                BrainTreeGateWay = CreateGateWay();
            }

            return BrainTreeGateWay;
        }
    }
}