using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Nop.Plugin.Shipping.ShipRocket.Models
{
    public class ShipRocketOrder
    {
        /// <summary>
        /// The order id you want to specify to the order. Max char: 20.  (Avoid passing character values as this contradicts some other API calls).
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// The date of order creation in yyyy-MM-dd HH:mm format. Time is an additional option.
        /// </summary>
        [JsonProperty("order_date")]
        public string OrderDate { get; set; }

        /// <summary>
        /// The pickup location added in your Shiprocket account. This cannot be a new location.
        /// </summary>
        [JsonProperty("pickup_location")]
        public string PickupLocation { get; set; }

        /// <summary>
        /// Id of the desired channel in case particular channel is to be selected.  Deafult is 'Custom'.
        /// </summary>
        [JsonProperty("channel_id")]
        public int? ChannelId { get; set; }

        /// <summary>
        /// Option to add 'From' field to the shipment. To do this, enter the name in the following format: 'Reseller: [name]'.
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// The 'from' name if you want to print. Use 'Reseller: [name]'
        /// </summary>
        [JsonProperty("reseller_name")]
        public string ResellerName { get; set; }

        /// <summary>
        /// Name of the company.
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// First name of the customer who is billed.
        /// </summary>
        [JsonProperty("billing_customer_name")]
        public string BillingCustomerFirstName { get; set; }

        /// <summary>
        /// Last name of the billed customer.
        /// </summary>
        [JsonProperty("billing_last_name")]
        public string BillingCustomerLastName { get; set; }

        /// <summary>
        /// address details of the billed customer.
        /// </summary>
        [JsonProperty("billing_address")]
        public string BillingAddress { get; set; }

        /// <summary>
        /// Further address details of the billed customer.
        /// </summary>
        [JsonProperty("billing_address_2")]
        public string BillingAddress2 { get; set; }

        /// <summary>
        /// Billing address city.  Max char: 30.
        /// </summary>
        [JsonProperty("billing_city")]
        public string BillingCity { get; set; }

        /// <summary>
        /// Pincode of the billing address.
        /// </summary>
        [JsonProperty("billing_pincode")]
        public string BillingPinCode { get; set; }

        /// <summary>
        /// Billing address state.
        /// </summary>
        [JsonProperty("billing_state")]
        public string BillingState { get; set; }

        /// <summary>
        /// Billing address country.
        /// </summary>
        [JsonProperty("billing_country")]
        public string BillingCountry { get; set; }

        /// <summary>
        /// Email address of the billed customer.
        /// </summary>
        [JsonProperty("billing_email")]
        public string BillingEmail { get; set; }

        /// <summary>
        /// The phone number of the billing customer.
        /// </summary>
        [JsonProperty("billing_phone")]
        public string BillingPhoneNumber { get; set; }

        /// <summary>
        /// Alternate phone number of the billing customer.
        /// </summary>
        [JsonProperty("billing_alternate_phone")]
        public long BillingAlternatePhoneNumber { get; set; }

        /// <summary>
        /// Whether the shipping address is the same as billing address. 1 or 'true' for yes and 0 or 'false' for no.
        /// </summary>
        [JsonProperty("shipping_is_billing")]
        public bool IsShippingAddressSameAsBilling { get; set; }

        /// <summary>
        /// Name of the customer the order is shipped to.   Required in case billing is not same as shipping.
        /// </summary>
        [JsonProperty("shipping_customer_name")]
        public string ShippingCustomerFirstName { get; set; }

        /// <summary>
        /// Last name of the shipping customer.
        /// </summary>
        [JsonProperty("shipping_last_name")]
        public string ShippingCustomerLastName { get; set; }

        /// <summary>
        /// Address of the Shipping customer. Required in case billing is not same as shipping.
        /// </summary>
        [JsonProperty("shipping_address")]
        public string ShippingAddress { get; set; }

        /// <summary>
        /// Further address details of shipping customer.
        /// </summary>
        [JsonProperty("shipping_address_2")]
        public string ShippingAddress2 { get; set; }

        /// <summary>
        /// ISD code of the billing address.
        /// </summary>
        [JsonProperty("billing_isd_code")]
        public string BillingIsdCode { get; set; }

        /// <summary>
        /// Shipping address city
        /// </summary>
        [JsonProperty("shipping_city")]
        public string ShippingCity { get; set; }

        /// <summary>
        /// Shipping address pincode.
        /// </summary>
        [JsonProperty("shipping_pincode")]
        public string ShippingPinCode { get; set; }

        /// <summary>
        /// Shipping address country
        /// </summary>
        [JsonProperty("shipping_country")]
        public string ShippingCountry { get; set; }

        /// <summary>
        /// Shipping address state.
        /// </summary>
        [JsonProperty("shipping_state")]
        public string ShippingState { get; set; }

        /// <summary>
        /// Email of the shipping customer.
        /// </summary>
        [JsonProperty("shipping_email")]
        public string ShippingEmail { get; set; }

        /// <summary>
        /// Phone no. of the shipping customer.
        /// </summary>
        [JsonProperty("shipping_phone")]
        public string ShippingPhone { get; set; }

        /// <summary>
        /// Array containing further fields.
        /// </summary>
        [JsonProperty("order_items")]
        public List<ShipRocketOrderItems> OrderItems { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        [JsonProperty("name")]
        public string ProductName { get; set; }

        /// <summary>
        /// The sku id of the product.
        /// </summary>
        [JsonProperty("sku")]
        public string ProductSKU { get; set; }

        /// <summary>
        /// No of units that are to be shipped.
        /// </summary>
        [JsonProperty("units")]
        public int Quantity { get; set; }

        /// <summary>
        /// Selling Price inclusive of GST
        /// </summary>
        [JsonProperty("selling_price")]
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// Discount should be inclusive of Tax
        /// </summary>
        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        /// <summary>
        /// The tax percentage on the item.
        /// </summary>
        [JsonProperty("tax")]
        public decimal TaxPercentage { get; set; }

        /// <summary>
        /// Harmonised System Nomenclature code. Used to determine the category of taxation the goods fall under.
        /// </summary>
        [JsonProperty("hsn")]
        public int HSNCode { get; set; }

        /// <summary>
        /// The method of payment. Can be either COD (Cash on delivery) Or Prepaid.
        /// </summary>
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Shipping charges if any in Rupee.
        /// </summary>
        [JsonProperty("shipping_charges")]
        public decimal ShippingCharges { get; set; }

        /// <summary>
        /// Giftwrap charges if any in Rupee.
        /// </summary>
        [JsonProperty("giftwrap_charges")]
        public decimal GiftWrapCharges { get; set; }

        /// <summary>
        /// Transaction charges if any in Rupee.
        /// </summary>
        [JsonProperty("transaction_charges")]
        public decimal TransactionCharges { get; set; }

        /// <summary>
        /// The total discount amount in Rupee.
        /// </summary>
        [JsonProperty("total_discount")]
        public decimal TotalDiscount { get; set; }

        /// <summary>
        /// Calculated sub total amount in Rupee after deductions.
        /// </summary>
        [JsonProperty("sub_total")]
        public decimal SubTotal { get; set; }

        /// <summary>
        /// The length of the item in cms. Must be more than 0.5.
        /// </summary>
        [JsonProperty("length")]
        public decimal ProductLength { get; set; }

        /// <summary>
        /// The breadth of the item in cms. Must be more than 0.5.
        /// </summary>
        [JsonProperty("breadth")]
        public decimal ProductBreadth { get; set; }

        /// <summary>
        /// The height of the item in cms. Must be more than 0.5.
        /// </summary>
        [JsonProperty("height")]
        public decimal ProductHeight { get; set; }

        /// <summary>
        /// The weight of the item in kgs. Must be more than 0.
        /// </summary>
        [JsonProperty("weight")]
        public decimal ProductWeight { get; set; }

        /// <summary>
        /// Details relating to the shipment of goods.                           .
        /// </summary>
        [JsonProperty("ewaybill_no")]
        public string EWayBillNumber { get; set; }

        /// <summary>
        /// Goods and Services Tax Identification Number.
        /// </summary>
        [JsonProperty("customer_gstin")]
        public string CustomerGSTNumber { get; set; }
    }

    public class ShipRocketOrderItems
    {

    }
}
