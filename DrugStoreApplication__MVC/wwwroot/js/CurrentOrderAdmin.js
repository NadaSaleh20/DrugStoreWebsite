
$(document).ready(function () {
    var UpdatesOrdes = [];
    console.log(UpdatesOrdes);
  /*  var minDate, maxDate;*/
    var curentOrderAdminVM =
    {
        drugId: 0,
        minPrice: 0,
        maxPrice: 0,
        pharmcyId: null,
        minDate: null,
        maxDate : null
    };

    /*--------------------------------------------------- Declartion DataTabel-------------------------------------*/
    var currentOrder = $('#CurrentOrderAdmin').DataTable({
        "ajax": {
            url: `/Admin/GetCurrentOrderAtAdmin`,
            type: 'POST',
            data: function (d) {
                d.drugId = curentOrderAdminVM.drugId,
                d.minPrice = curentOrderAdminVM.minPrice,
                    d.maxPrice = curentOrderAdminVM.maxPrice,
                    d.pharmcyId = curentOrderAdminVM.pharmcyId,
                    d.minDate = curentOrderAdminVM.minDate,
                    d.maxDate = curentOrderAdminVM.maxDate
            },
            /*contentType: "application/json",*/
            dataSrc: ""

        },
        "columnDefs": [{
            targets: [0, 6, 7, 8],
            visible: false
        }],
        "columns": [
            { 'data': 'dateString' },
            { 'data': 'drugID', 'className': 'genralname' },
            { 'data': 'drugName', 'className': 'drugname' },
            { "data": 'quantity', 'className': 'genralname' },
            { 'data': 'pricePerUnit', 'className': 'price' },
            { 'data': 'totalPriceRow', 'className': 'price' },
            { 'data': 'pharmcyName' },
            { 'data': 'totalPrice' },
            { 'data': 'fInalOrderId' }



        ],

        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ordering": false,
        "info": false,
        "lengthChange": false,
        "paging": true,
        'colReorder': true,


        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            var columns = [6, 0, 7];

            for (c = 0; c < columns.length; c++) {
                var colNo = columns[c];
                if (colNo == 6) {
                    api.column(colNo, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                `<tr  style="background-color: #E3E3E3;" >
                              <td colspan="7" class="text-left px-5"style="font-weight: 500;color: #168305;font-size: 20px;"> ${api.column(6).data()[i]} </td>
                                </tr>`


                            );

                            last = group;
                        }
                    });


                }
                else if (colNo == 0) {
                    api.column(colNo, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                `<tr class="group font-weight-bold "style="background-color: #F9F9F9;">
                                <td class="text-left  px-5" colspan="6" style="color: #707070;  font-family: Roboto;font-size: 15px;">
                                  ${api.column(0).data()[i]}</td>
                                 <td colspan="1" style="color: #707070;">
                                  <button class="btn p-0 edit" style="margin-left: 13px;"
                                     data-id="${api.column(8).data()[i]}" data-name="${api.column(6).data()[i]}"
                                      data-toggle="modal" data-target="#myModal"
                                  >
                                  <i class="fa-solid fa-pen"></i>
                                     </button>
                                 </td>
                                      </tr>`

                            );
                            last = group;
                        }

                    });
                }
                else {

                    api.column(colNo, { page: 'current' }).data().each(function (group, i) {

                        if (last !== group) {
                            $(rows).eq(i).before(
                                `<tr  style="background-color: #D4EDF9" >
                                  <td colspan="5" class="text-left  px-5" style="color: #707070; font-weight: bold;">Total Price: <span style="color: #168305;
                                        font-size: 15px;">${api.column(7).data()[i]}NIS</span></td>
                                 <td colspan="2">                                                                       
                                <input type="checkbox" class="processed" style="width:20px;heigth:20px;" data-id='${api.column(6).data()[i]}' data-name="${api.column(8).data()[i]}" />
                               <span style="color:#707070;font-family:Roboto;font-weight: bold;">Mark as processed<span></td>
                              </tr>`

                            );

                            last = group;
                        }
                    });
                }

            }
        },

    });


    /*------------------------------------------------------------------  Process Orders------------------------------------------------------*/

    $('#CurrentOrderAdmin').on('click', 'tbody td .processed', function (e) {
        var pharmcyName = $(this).data('id');
        var finalOrderId = $(this).data('name');
        Swal.fire({
            title: 'Do you want to Confirm this Order?',
            showDenyButton: true,
            confirmButtonText: 'Yes',
            denyButtonText: `No`,
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Admin/ConfirmOrders',
                    data: {
                        finalOrderId: finalOrderId,
                        PharmcyName: pharmcyName
                    },
                    success: function (result) {
                        location.href = "https://localhost:44356/Admin/ArchivedOrderAdmin";
                    }
                });

            }
            else if (result.isDenied) {
                Swal.fire('Changes are not saved', '', 'info')
            }
        });
 
    });


    /* ----------------------------------------------------------  Edit Order--------------------------------------------------------------------*/
    
    $('#CurrentOrderAdmin').on('click', 'tbody td .edit', function (e) {
  
        UpdatesOrdes = [];
        console.log(UpdatesOrdes);
        var finalOrderId = $(this).data('id');
        var pharmcyName = $(this).data('name');
        $.ajax({
            url: '/Admin/GetOrdersBeEditedByAdmin',
            data:
            {
                finalOrderId: finalOrderId,
                pharmcyName: pharmcyName
            },
            success: function (result) {
                $("#tbodyid").empty();
                var TabelBody = "";
                for (let order of result) {
                    TabelBody += `<tr>
                                <td class='drugId'>${order.drugID}</td>
                                <td>${order.drugName}</td>
                                 <td><input type="number" min="1" class="quantity" value="${order.quantity}" style="width: 50px;"/> </td>
                                <td class="price">${order.pricePerUnit}</td>
                                 <td class="TotalPrice">${order.totalPriceRow}</td>
                                 <td class="orderId" style="display:none;">${order.orderId}</td>
                                   <td class="pharmcyName" style="display:none;">${order.pharmcyName}</td>
                                   <td class="finalOrderId" style="display:none;">${order.fInalOrderId}</td>
                                  </tr>`;
                     
                }
                console.log(TabelBody);
                $('#editTabel').find('tbody').append(TabelBody); 
            }
        });
    });

    $("#editTabel").on("change", "tr td .quantity", function () {
       
        var quantityNew = $(this).val();
        if (quantityNew <= 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'please, Enter the Quantity grather than zero',
            });
        }
        else {
            var finalOrderId = $(this).closest("tr").find(".finalOrderId").text();
            var drugId = $(this).closest("tr").find(".drugId").text();
            var price = $(this).closest("tr").find(".price").text();
            var newTotalPrice = price * quantityNew;
            $(this).closest("tr").find(".TotalPrice").text(newTotalPrice);
            var orderId = $(this).closest("tr").find(".orderId").text();
            var pharmcyName = $(this).closest("tr").find(".pharmcyName").text();
            var UpdateOrderVM = {};
           
                UpdateOrderVM.drugId =drugId,
                UpdateOrderVM.orderId =orderId,
                UpdateOrderVM.QuaNew = quantityNew,
                UpdateOrderVM.pharmcyName = pharmcyName,
                UpdateOrderVM.finalOrderId =finalOrderId
          
            UpdatesOrdes.push(UpdateOrderVM);
        
        }
    });
     /*---------------------------------------------------------------- Update Button---------------------------------------------------*/
    $('#UpdateButton').on('click', function (e) {
        debugger
        console.log(UpdatesOrdes);
        $.ajax({
            type: 'POST',
            url: '/Admin/UpdateOrdres',
            data: JSON.stringify(UpdatesOrdes),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result == 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Opss ,No Orders Updated ',
                        confirmButtonText: 'Ok',
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.href = "https://localhost:44356/Admin/CurrentOrderAdmin";
                        }
                    });
                }
                else if (result == 1) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Opss ,New Quantity is Larger Than Qunatity Storge ',
                        confirmButtonText: 'Ok',
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.href = "https://localhost:44356/Admin/CurrentOrderAdmin";
                        }
                    });

                }
                else {
                    Swal.fire({
                        icon: 'success',
                        title: 'Oops...',
                        title: 'Order Updated successfully',
                        confirmButtonText: 'Ok',
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.href = "https://localhost:44356/Admin/CurrentOrderAdmin";
                        }
                    });
                }
             
            }
        });

    });
    /*----------------------------------------------------------- DrugName Filter----------------------------------------------------*/
    $('.filter-drug').select2({
        placeholder: `<i class="fas fa-notes-medical mr-2"></i>Drug Name`,

        escapeMarkup: function (markup) { return markup; },
        ajax: {
            url: '/home/GetDrugs',
            data: function (params) {
                return {
                    search: params.term,
                    page: params.page
                };
            },
            processResults: function (data) {
                var formmatedData = data.map(x => ({ id: x.drugId, text: x.drugName }));
                return {
                    results: formmatedData
                };
            },

            minimumInputLength: 2,
            width: 'resolve'
        }

    });

    $('.filter-drug').on('select2:select', function (e) {
        curentOrderAdminVM.drugId = e.params.data.id;
        currentOrder.ajax.reload();
        curentOrderAdminVM.drugId = 0;
        $(".filter-drug").val('').trigger('change');
    });
    /* ------------------------------------------------------Input Date Picker----------------------------------------------------------------------------*/
    // Create date inputs
    minDate = $('#min').datepicker(
        {
          /*  format: 'dd/mm/yyyy'*/
        }).on('changeDate', function (e) {
       
        });

    maxDate = $('#max').datepicker({
      /*  format: 'dd/mm/yyyy',*/
    }).on('changeDate', function (e) {
        debugger
        console.log(minDate.val())
        curentOrderAdminVM.minDate = minDate.val();
        curentOrderAdminVM.maxDate = maxDate.val();
        console.log(maxDate.val())
        currentOrder.ajax.reload();
    });

    /*------------------------------------------------ pharmcy Filtring-------------------------------------------------*/
    $('.filter-Pharmcy').select2({
        placeholder: `<i class="fas fa-notes-medical mr-2"></i>Pharmcies`,
        escapeMarkup: function (markup) { return markup; },
        ajax: {
            url: '/Admin/SelectPharmcies',
            data: function (params) {
                return {
                    search: params.term,
                    page: params.page
                };
            },
            processResults: function (data) {
                var formmatedData = data.map(x => ({ id: x.pharmcyId, text: x.pharmcyName }));
                return {
                    results: formmatedData
                };
            },

            minimumInputLength: 2,
            width: 'resolve'
        }

    });
    $('.filter-Pharmcy').on('select2:select', function (e) {
        debugger

        curentOrderAdminVM.pharmcyId = e.params.data.id;
        currentOrder.ajax.reload();
        curentOrderAdminVM.pharmcyId = null;
        $(".filter-Pharmcy").val('').trigger('change');
      
    });

    /*------------------------------------------------------    MinAndMaxPrice Filtering----------------------------*/

    $('body').on('change', '#SecondInput', function (e) {
        debugger
      
        var minPrice = parseInt($('#FirstInput').val());
        var maxPrice = parseInt($('#SecondInput').val());
     

       if (minPrice > maxPrice) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Opss , Min Price is larger than Max Price ',
            });

        }
       else
       {
            curentOrderAdminVM.minPrice = minPrice;
            curentOrderAdminVM.maxPrice = maxPrice;
            currentOrder.ajax.reload();
       }
        $('#FirstInput').val("");
        $('#SecondInput').val("");
    });
});



   



 
