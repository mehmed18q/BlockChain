pragma solidity ^0.5.9;
contract RealEstateToken {
    
    mapping (address => uint) _balances;
    address[] _customers;
    
    struct EsContractStr {
        address buyer;
        address seller;
        address owner;
        uint256 contractAmount;
        bool buyerOK;
        bool sellerOK;
    }
    
    mapping (uint => EsContractStr) EsContractStrs;
    uint[] public EsContractStrsCodes;
    
    constructor() public payable {
    }
    
    function addCustomer (address accountToAdd) private returns (bool)
    {
        require(!customerExist(accountToAdd));
        _customers.push(accountToAdd);
        assert(customerExist(accountToAdd));
        return true;
    }
    
    function customerExist(address accountToCheck) public view returns(bool)
    {
        for (uint i = 0; i < _customers.length; i++){
        if (_customers[i] == accountToCheck)
        return true;
        }
        return false;
    }
    
    function getBalance(address tokenOwner) public view returns (uint)
    {
        return _balances[tokenOwner];
    }
    
    function addEstateContract(uint ecCode, address buyer, address seller, uint amount) public returns (bool){
        EsContractStrsCodes.push(ecCode);
        EsContractStr storage c = EsContractStrs[ecCode];
        c.buyer = buyer;
        c.seller = seller;
        c.contractAmount = amount;
        c.buyerOK = false;
        c.sellerOK = false;
        
        _balances[buyer] += amount;
        addCustomer(buyer);
        addCustomer(seller);
        
        return true;
    }
    
    function transferFrom (address sender, address receiver, uint transferAmount) public returns (bool)
    {
        require (customerExist(sender), "Sender is not customer.");
        require (customerExist(receiver), "Receiver is not customer.");
        require (_balances[sender] >= transferAmount , "Balance not enough.");
        
        _balances[sender] = _balances[sender] - transferAmount;
        _balances[receiver] = _balances[receiver] + transferAmount;
        
        return true;
    }
    
    function accept(uint ecCode) public returns (bool) {
        if (msg.sender == EsContractStrs[ecCode].buyer)
        EsContractStrs[ecCode].buyerOK = true;
        else if (msg.sender == EsContractStrs[ecCode].seller)
        EsContractStrs[ecCode].sellerOK = true;
        
        if (EsContractStrs[ecCode].buyerOK && EsContractStrs[ecCode].sellerOK)
        {
            transferFrom(EsContractStrs[ecCode].buyer, EsContractStrs[ecCode].seller, EsContractStrs[ecCode].contractAmount);
        }
    }
    
    function getOwner (uint ecCode) public view returns (address owner){
     return EsContractStrs[ecCode].owner;
    }
    
    }
