/*********************************************************
       Code Generated By  .  .  .  .  Delaney's Script Bot
       WWW .  .  .  .  .  .  .  .  .  www.scriptbot.io
       Version.  .  .  .  .  .  .  .  10.0.0.14 
       Build  .  .  .  .  .  .  .  .  20191113 216
       Template Name.  .  .  .  .  .  Project Green 2.0
       Template Version.  .  .  .  .  20191030 002
       Author .  .  .  .  .  .  .  .  Delaney

                      ,        ,--,_
                       \ _ ___/ /\|
                       ( )__, )
                      |/_  '--,
                        \ `  / '
 
 Note: Do not send this object to the view.
       Use objects create from Models classes instead.
       This class in a representation of data objects.
 
 *********************************************************/
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Data.Core.Domain
{
    [XmlType(Namespace = "")]
    public partial class FormationPaypal : Services.Data.Core.Domain.Abstract
    {
        public FormationPaypal()
        {
            _createdDate = System.DateTime.Now;
        }
        #region Properties
        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        #region CurrencyCode
        private string _currencyCode = "";
        public string CurrencyCode
        {
            get
            {
                return _currencyCode;
            }
            set
            {
                if(value != null)
                    value = value.Trim();

                if (_currencyCode != value)
                {
                    _currencyCode = value;
                    IsSaved = false;
                }
            }
        }
        #endregion

        #region CurrencySymbol
        private string _currencySymbol = "";
        public string CurrencySymbol
        {
            get
            {
                return _currencySymbol;
            }
            set
            {
                if(value != null)
                    value = value.Trim();

                if (_currencySymbol != value)
                {
                    _currencySymbol = value;
                    IsSaved = false;
                }
            }
        }
        #endregion

        #region Discount
        private decimal _discount;
        public decimal Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                if (_discount != value)
                {
                    _discount = value;
                    IsSaved = false;
                }
            }
        }
        #endregion

        [IgnoreDataMember]
        [XmlIgnore]
        public List<Product> Products { get; set; } = new List<Product>();

        [IgnoreDataMember]
        [XmlIgnore]
        public decimal SubTotal
        {
            get
            {
                decimal total = 0;

                foreach (var product in Products)
                    total += product.Price;

                return total;
            }
        }


        [IgnoreDataMember]
        [XmlIgnore]
        public decimal Total
        {
            get
            {
                decimal total = SubTotal;
                total -= Discount;
                return total;
            }
        }
        #endregion

        #region Methods
        public override void Update(Abstract entity)
        {
            var basket = (FormationPaypal)entity;

            CurrencyCode = basket.CurrencyCode;
            CurrencySymbol = basket.CurrencySymbol;
            Discount = basket.Discount;
        }
        #endregion
    }
}