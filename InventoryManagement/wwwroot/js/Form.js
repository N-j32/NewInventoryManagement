$(document).ready(function () {
    $('#orderDate').datepicker({ dateFormat: 'dd/mm/yy' });
    $('#orderDate').datepicker('setDate', new Date());
});

//reset form
$(document).ready(function () {

    $("#reset").click(function () {
        $(this).closest('table').find("input[type=text], textarea").val("");
    });
});
//$(document).ready(function () {

//    $("#remove").click(function () {
//        $('#orderdetailsItems').find('tbody').detach();
//    });
//});
//reset grid
//$("#orderdetailsItems").on('click', '#remove', function () {
//    $(this).closest('tr').remove();
//});

$(document).ready(function () {
    GrandTotal = 0
    $('#remove').click(function () {
        $("#orderItems").find("tr:gt(0)").remove();
        $("#lblGrandTotal").html(GrandTotal);
        location.reload();

    });
});


$(function () {
    $("#automplete").autocomplete({
        source: function (req, res) {
            $.ajax({
                url: "/Form/AutoComplete",
                dataType: "json",
                success: function (data) {
                    console.log(data)
                    res(data);
                }
            })
        }
    });
});


//$(document).ready(function () {

//    $("#itemName").autocomplete({

//        source: function (request, response) {

//            $.ajax({

//                url: "/Form/AutoComplete",

//                type: "POST",

//                dataType: "json",

//                data: { prefix: request.term },

//                success: function (data) {
//                    console.log(response)
//                    console.log(response(data));
//                    response($.map(data, function (item) {



//                        return { label: item.Name/*, key: item.ProductId*/};

//                    }))

//                }

//            })

//        },
//        minLength: 3,
//        select: function (event, ui) {

//            $("#itemName").val(ui.item.label); // display the selected text
//            $("#ProductId").val(ui.item.value); // save selected id to hidden input
//            return false;
//        }

//    });


//});
$(document).ready(function () {
    var orderItems = [];
    //Add button click function
    $('#add').click(function () {
        //Check validation of order item
        var isValidItem = true;
        if ($('#itemName').val().trim() == '') {
            isValidItem = false;
            $('#itemName').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#itemName').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#quantity').val().trim() != '' && !isNaN($('#quantity').val().trim()))) {
            isValidItem = false;
            $('#quantity').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#quantity').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#rate').val().trim() != '' && !isNaN($('#rate').val().trim()))) {
            isValidItem = false;
            $('#rate').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#rate').siblings('span.error').css('visibility', 'hidden');
        }


        //Add item to list if valid
        if (isValidItem) {
            orderItems.push({
                Product: $('#itemName').val().trim(),
                Quantity: parseInt($('#quantity').val().trim()),
                Rate: parseFloat($('#rate').val().trim()),
                TotalAmount: parseInt($('#quantity').val().trim()) * parseFloat($('#rate').val().trim()),
                ProductId: $('#ProductId').val().trim()

            });

            //Clear fields
            $('#itemName').val('').focus();
            $('#quantity,#rate').val('');

        }


        //populate order items
        GeneratedItemsTable();
        //calculate grandtotal
        /*CalculateGrandTotal();*/

    });
    //Save button click function
    $('#submit').click(function () {
        //validation of order
        var isAllValid = true;
        if (orderItems.length == 0) {
            $('#orderItems').html('<span style="color:red;">Please add order items</span>');
            isAllValid = false;
        }



        if ($('#orderDate').val().trim() == '') {
            $('#orderDate').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#orderDate').siblings('span.error').css('visibility', 'hidden');
        }



        //Save if valid
        if (isAllValid) {
            var data = {

                OrderDate: $('#orderDate').val().trim(),
                SupplierId: $('#supplier').val(),
                GrandTotal: $('#lblGrandTotal').text(),
                ProductId: $('#ProductId').val(),
                OrderDetails: orderItems
            }

            $(this).val('Please wait...');

            //$.ajax(
            //         {
            //             type: 'POST',
            //             dataType: 'JSON',
            //             url: '/Form/GetData',
            //             data: { jsonInput: JSON.stringify(data) },
            //             success:
            //                 function (response) {
            //                     // Generate HTML table.  
            //                     convertJsonToHtmlTable(JSON.parse(response), $("#TableId"));
            //                 },
            //             error:
            //                 function (response) {
            //                     alert("Error: " + response);
            //                 }
            //    });
            // $.ajax({



            //     type: 'POST',
            //     dataType: 'JSON',
            //     url: '/Form/test',
            //     data: data,
            //     contentType: 'application/x-www-form-urlencoded',
            //     success: function (d) {
            //         //check is successfully save to database
            //         if (d.status == true) {
            //             //will send status from server side
            //             alert('Successfully done.');
            //             //clear form
            //             orderItems = [];



            //             $('#orderDate').val('');
            //             $('#orderItems').empty();
            //         }
            //         else {
            //             alert('Failed');
            //         }
            //         $('#submit').val('Save');
            //     },
            //     error: function () {
            //         alert('Error. Please try again.');
            //         $('#submit').val('Save');
            //     }
            // });




            $.ajax({

                type: 'POST',
                dataType: 'JSON',
                url: '/Form/SaveOrder',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (d) {


                    //check is successfully save to database
                    if (d.status == true || d.response == success) {
                        //alert(d);
                        //will send status from server side
                        //alert('Successfully done.');
            //            if (@TempData["message"] == "Added"){
            //    toastr.success('Successfully saved');
            //}
            //                    else { }
            /* $("#orderdetailsItems")[0].reset();*/
            alert(d.responseText);
            orderItems = [];


            GrandTotal = '';
            //$('#orderDate').val('');
            $('#orderItems').val('');




        }
        else {
            /*alert('Failed');*/
            alert(d.responseText);
        }
        $('#submit').val('Save');

    },
        error: function (d) {
            alert('Error');
            // alert('Error. Please try again.');
            //setTimeout("$('#success').dialog('close');", 3000);
            //$('#orderdetailsItems"').msgpopup({
            //    text: 'Error. Please try again.',
            //});
            $('#submit').val('Save');
        }
                    });


                }


            });





function GeneratedItemsTable() {

    if (orderItems.length >= 0) {
        var $table = $('<table/>');
        $table.append('<thead><tr><th>Item</th><th>Quantity</th><th>Rate</th><th>Total Amount</th><th>Action</th></tr></thead>');
        var $tbody = $('<tbody/>');
        var GrandTotal = 0;

        $.each(orderItems, function (i, val) {
            var $row = $('<tr/>');
            $row.append($('<td/>').html(val.Product));
            $row.append($('<td/>').html(val.Quantity));
            $row.append($('<td/>').html(val.Rate));
            $row.append($('<td/>').html(val.TotalAmount));

            GrandTotal += val.TotalAmount;

            var $remove = $('<a href="#">Remove</a>');
            $remove.click(function (e) {
                e.preventDefault();
                orderItems.splice(i, 1);
                GeneratedItemsTable();


            });
            $row.append($('<td/>').html($remove));
            $tbody.append($row);
        });
        $('#lblGrandTotal').html(GrandTotal.toFixed(2));
        console.log("current", orderItems);
        $table.append($tbody);
        $('#orderItems').html($table);

    }

    else {
        $('#orderItems').html('');
    }

}


       

