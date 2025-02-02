using Mediwatch.Server.Areas.Identity.Data;
using Mediwatch.Server.PayPal.Values;
using Mediwatch.Shared.Models;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System.Collections.Generic;

namespace Mediwatch.Server.PayPal
{
    public class OrderBuilder
    {
        /// <summary>
        /// Use classes from the PayPalCheckoutSdk to build an OrderRequest
        /// </summary>
        /// <returns></returns>

#region save
        public static OrderRequest Build(formation currentForm)
        {
            #region testRegion
            // OrderRequest orderRequest = new OrderRequest()
            // {
            //     CheckoutPaymentIntent = CheckoutPaymentIntent.CAPTURE,
            //     ApplicationContext = new ApplicationContext
            //     {
            //         BrandName = "Delaneys.space",
            //         LandingPage = PayPal.Values.LandingPage.LOGIN,
            //         UserAction = PayPal.Values.UserAction.PAY_NOW,
            //         ShippingPreference = PayPal.Values.ShippingPreference.NO_SHIPPING,
            //         Locale = "fr-FR"
            //     },
            //     PurchaseUnits = new List<PurchaseUnitRequest>
            //     {
            //         new PurchaseUnitRequest
            //         {
            //             ReferenceId = "default",//modify
            //             Description = "Mediwatch formation",
            //             SoftDescriptor = "Mediwatch",
            //              AmountWithBreakdown = new AmountWithBreakdown
            //             {
            //                 CurrencyCode = PayPal.Values.CurrencyCode.EUR,
            //                 Value = "42.66",
            //                 // AmountBreakdown = new AmountBreakdown
            //                 // {
            //                 //     ItemTotal = new Money
            //                 //     {
            //                 //         CurrencyCode = basket.CurrencyCode,
            //                 //         Value = basket.SubTotal.ToString()
            //                 //     },
            //                 // }
            //             },
            //         }
            //     }
            // };

            // orderRequest.PurchaseUnits[0].Items.Add(new Item 
            // {
            //     Name = "Formation PLACEHOLDER",
            //     Description = "This is a PLACEHOLDER",
            //     UnitAmount = new Money {
            //          CurrencyCode = PayPal.Values.CurrencyCode.EUR,
            //          Value = "42.66"
            //     },
            //     Quantity = "1",
            //     Category = PayPal.Values.Items.Category.DIGITAL_GOODS
            // });




            //Panier implémentation
            // foreach (var product in basket.Products)
            // {
            //     orderRequest.PurchaseUnits[0]
            //                 .Items
            //                 .Add(new Item
            //                 {
            //                     Name = product.Name,
            //                     Description = product.Description,
            //                     UnitAmount = new Money
            //                     {
            //                         CurrencyCode = basket.CurrencyCode,
            //                         Value = product.Price.ToString()
            //                     },
            //                     Quantity = product.Quantity.ToString(),
            //                     Category = PayPal.Values.Item.Category.DIGITAL_GOODS
            //                 });
            // }
            #endregion

            string priceStr = FormatPrice(currentForm.Price);
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                            Value = priceStr
                        }
                    }
                }
            };
            return order;
        }

             public static OrderRequest Build(List<formation> listCurrentForm)
        {
            var priceGet = GetPrice(listCurrentForm);
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        // AmountWithBreakdown = new AmountWithBreakdown()
                        // {
                        //     CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                        //     Value = priceGet
                        // }
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                            Value = priceGet,
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                                    Value = priceGet
                                }
                            },
                        },
                        // Items = GetItems(listCurrentForm)
                    }
                }
            };
            return order;
        }
#endregion
        public static OrderRequest Build(List<formation> listCurrentForm, UserCustom userCustom)
        {
            List<Item> ItemFormation = GetListItems(listCurrentForm);
            var itemTotal = GetItemTotal(listCurrentForm);
            string inputValue = GetValue(listCurrentForm);
            OrderRequest orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                ApplicationContext = new ApplicationContext
                {
                    BrandName = "MEDIWATCH",
                    LandingPage = "BILLING",
                    UserAction = "CONTINUE",
                    ShippingPreference = "NO_SHIPPING"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        ReferenceId =  "PUHF",
                        Description = "Formation",
                        CustomId = "MEDIWATCHID",
                        SoftDescriptor = "Formation",
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "EUR",
                            Value = inputValue, //TOTAL ItemTotal, Shipping, Handling, Shipping discount
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = itemTotal,
                                Shipping = new Money
                                {
                                    CurrencyCode = "EUR",
                                    Value = "0.00"
                                },
                                Handling = new Money
                                {
                                    CurrencyCode = "EUR",
                                    Value = "0.00"
                                },
                                TaxTotal = new Money
                                {
                                    CurrencyCode = "EUR",
                                    Value = "0.00"
                                },
                                ShippingDiscount = new Money
                                {
                                    CurrencyCode = "EUR",
                                    Value = "0.00"
                                }
                            }
                        },
                        Items = ItemFormation,
                        ShippingDetail = new ShippingDetail
                        {
                        Name = new Name
                        {
                            FullName = userCustom.UserName
                        }
                        }
                    }
                }
            };

            // OrderRequest orderRequest = new OrderRequest()
            // {
            //     CheckoutPaymentIntent = "CAPTURE",
            //     ApplicationContext = new ApplicationContext
            //     {
            //         BrandName = "MEDIWATCH",
            //         LandingPage = "BILLING",
            //         UserAction = "CONTINUE",
            //         ShippingPreference = "NO_SHIPPING"
            //     },
            //     PurchaseUnits = new List<PurchaseUnitRequest>
            //     {
            //         new PurchaseUnitRequest
            //         {
            //             ReferenceId =  "PUHF",
            //             Description = "Formation",
            //             CustomId = "MEDIWATCHID",
            //             SoftDescriptor = "Formation",
            //             AmountWithBreakdown = new AmountWithBreakdown
            //             {
            //                 CurrencyCode = "EUR",
            //                 Value = "230.00", //TOTAL ItemTotal, Shipping, Handling, Shipping discount
            //                 AmountBreakdown = new AmountBreakdown
            //                 {
            //                     ItemTotal = new Money
            //                     {
            //                         CurrencyCode = "EUR",
            //                         Value = "180.00"
            //                     },
            //                     Shipping = new Money
            //                     {
            //                         CurrencyCode = "EUR",
            //                         Value = "30.00"
            //                     },
            //                     Handling = new Money
            //                     {
            //                         CurrencyCode = "EUR",
            //                         Value = "10.00"
            //                     },
            //                     TaxTotal = new Money
            //                     {
            //                         CurrencyCode = "EUR",
            //                         Value = "20.00"
            //                     },
            //                     ShippingDiscount = new Money
            //                     {
            //                         CurrencyCode = "EUR",
            //                         Value = "10.00"
            //                     }
            //                 }
            //             },
            //             Items = new List<Item>
            //             {
            //                 new Item
            //                 {
            //                     Name = "T-shirt",
            //                     Description = "Green XL",
            //                     Sku = "ThisIS TEst",
            //                     UnitAmount = new Money
            //                     {
            //                         CurrencyCode = "EUR",
            //                         Value = "90.00"
            //                     },
            //                     Tax = new Money
            //                     {
            //                         CurrencyCode = "EUR",
            //                         Value = "10.00"
            //                     },
            //                     Quantity = "1",
            //                     Category = "DIGITAL_GOODS"
            //                 },
            //                 new Item
            //                     {
            //                     Name = "Shoes",
            //                     Description = "Running, Size 10.5",
            //                     Sku = "sku02",
            //                     UnitAmount = new Money
            //                         {
            //                             CurrencyCode = "EUR",
            //                             Value = "45.00"
            //                         },
            //                         Tax = new Money
            //                         {
            //                         CurrencyCode = "EUR",
            //                         Value = "5.00"
            //                         },
            //                         Quantity = "2",
            //                         Category = "DIGITAL_GOODS"
            //                     }
            //             }
            //             ,
            //             ShippingDetail = new ShippingDetail
            //             {
            //             Name = new Name
            //             {
            //                 FullName = userCustom.UserName
            //             }
            //             //,
            //             // AddressPortable = new AddressPortable
            //             // {
            //             //     AddressLine1 = "123 Townsend St",
            //             //     AddressLine2 = "Floor 6",
            //             //     AdminArea2 = "San Francisco",
            //             //     AdminArea1 = "CA",
            //             //     PostalCode = "94107",
            //             //     CountryCode = "US"
            //             // }
            //             // AddressPortable = new AddressPortable
            //             // {
            //             //     AddressLine1 = "",
            //             //     AddressLine2 = "Floor 6",
            //             //     AdminArea2 = "San Francisco",
            //             //     AdminArea1 = "CA",
            //             //     PostalCode = "94107",
            //             //     CountryCode = "US"
            //             // }

            //             }
            //         }
            //     }
            // };
            return orderRequest;
        }

        private static string GetValue(List<formation> listCurrentForm)
        {
            decimal valTmp = 0;
            foreach (var item in listCurrentForm)
            {
                valTmp += item.Price;
            }
            // valTmp += 3;
            return FormatPrice(valTmp);
        }

        private static List<Item> GetListItems(List<formation> listCurrentForm)
        {
            var ItemFormation = new List<Item>();
            foreach (var item in listCurrentForm)
            {
                var formToItem = new Item()
                {
                    Name = item.Name,
                    Description = "Formation à " + item.Location,
                    Sku = "Formateur: " + item.Former,
                    UnitAmount = new Money()
                    {
                        CurrencyCode = "EUR",
                        Value = FormatPrice(item.Price)
                    },
                    Tax = new Money()
                    {
                        CurrencyCode = "EUR",
                        Value = "1.00"
                    },
                    Quantity = "1",
                    Category = "DIGITAL_GOODS"
                };
            }

            return ItemFormation;
        }

        private static Money GetItemTotal(List<formation> itemFormation)
        {
            decimal tmpPrice = 0;
            foreach (var item in itemFormation)
            {
                tmpPrice += item.Price;
            }
            var result = new Money()
            {
                CurrencyCode = "EUR",
                Value = FormatPrice(tmpPrice)
            };
            return result;
        }

        private static List<Item> GetItems(List<formation> listCurrentForm)
        {
            var ListItems = new List<Item>();
            foreach (var itForm in listCurrentForm)
            {
                var newItem = new Item()
                {
                    Name = itForm.Name,
                    Quantity = "1",
                    Description = itForm.Description
                };
            }
            return ListItems;
        }
        private static string FormatPrice(decimal input)
        {
            return input.ToString().Replace(",", ".");
        }
        private static string GetPrice(List<formation> listCurrentForm)
        {
            decimal TotalPrice = 0;
            foreach (var curForm in listCurrentForm)
            {
                TotalPrice += curForm.Price;
            }
            return FormatPrice(TotalPrice);
        }
    }
}