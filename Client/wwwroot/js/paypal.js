let orderId;
let listId = [];

async function init_paypal(listId) {
    if (document.getElementById("paypal-button-container") != null) {
        paypal.Buttons({
            style: {
                layout:  'vertical',
                color:   'blue',
                shape:   'rect',
                label:   'checkout'
              },
            // Set up the transaction
            createOrder: function (data, actions) {
                orderId = data.orderID;
                jsonObj = JSON.stringify({
                    formationId: listId,
                    orderId: data.orderID
                })
                return fetch('/api/paypal/checkout/order/create/', {
                    method: 'post',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: jsonObj
                }).then(function (res) {
                    return res.json();
                }).then(function (data) {
                    return data.orderID;
                });
            },

            // Finalise the transaction
            onApprove: function (data, actions) {

                return fetch('/api/paypal/checkout/order/approved/' + data.orderID, {
                    method: 'post'
                }).then(function (res) {
                    return actions.order.capture();
                }).then(function (details) {

                    // (Preferred) Notify the server that the transaction id complete and have a option to display an order completed screen.
                    // httpGet('/api/paypal/checkout/order/complete/' +  data.orderID);
                    // httpPost('/api/paypal/checkout/order/complete/' +  data.orderID,
                    // {
                    //     formationId: id,
                    //     orderId: data.orderID
                    // })
                    // OR
                    // Notify the server that the transaction id complete
                    const invoiceId = httpGet('/api/paypal/checkout/order/complete/' + data.orderID);

                    console.log('================================')
                    console.log(invoiceId)

                    window.location.href = '/formations/fin?ids=' + listId.join(',') + '&facture=' + invoiceId;
                    // Show a success message to the buyer
                    console.log('Transaction completed by ' + details.payer.name.given_name + '!');
                });
            },

            // Buyer cancelled the payment
            onCancel: async function (data, actions) {
                httpGet('/api/paypal/checkout/order/cancel/' + data.orderID);
                console.error('Transaction cancel by ' + details.payer.name.given_name + '!');
            },

            // An error occurred during the transaction
            onError: function (err) {
                console.log('Error');
                console.log("[" + err + "]");
                httpGet('/api/paypal/checkout/order/error/' + orderId + '/' + encodeURIComponent(err));
            }

        }).render('#paypal-button-container');
    }
}

function httpGet(url) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET", url, false);
    xmlHttp.send(null);
    return xmlHttp.responseText;
}

function httpPost(url, payload) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", url, false);
    xmlHttp.send(JSON.stringify(payload));
}
