using Braintree;

namespace Rocky.Utils.BrainTree
{
    public interface IBrainTreeGate
    {
        IBraintreeGateway CreateGateWay();

        IBraintreeGateway GetGateWay();
    }
}