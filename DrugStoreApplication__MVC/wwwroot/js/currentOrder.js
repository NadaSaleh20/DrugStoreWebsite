
$(document).ready(function () {
    //Coulmn Index
    var DrugIdUpdate, quantityUpdate, orderId, finalOrderId, DrugUpdate ,Price;
    var groupColumn = 7;
    var groupColumn2 = 8;
    var UpdatesOrdes = [];
    //Delcaring dataTabel 
    var currentOrder = $('#CurrentOrderTabel').DataTable({
    //hide coulmn TotalPrice And Day
        columnDefs: [
            { visible: false, targets: groupColumn ,type: 'date'},
            { visible: false, targets: groupColumn2  }

        ],

        "ajax": {
            "url": '/Orders/GetCurrentOrder',
            "dataSrc": ""
        },

        "columns": [
            { 'data': 'drugID', "searchable": true },
            { 'data': 'drugName', 'className': "MangeDName" },
            { "data": 'quantity'},
            {
                'data': 'isAvialable',
                'render': function (data, type, row) {
                 return '<i class="fa fa-check-square" aria-hidden="true" style="color:#168305"></i>Avaliable' ;
            }},
            { 'data': 'pricePerUnit'},
            { 'data': 'totalPriceRow'},
            {
                'data': 'status',
                'render': function (data, type, row) {
                    return `<h7 style="color: #946800;font-weight: 700;margin-left: 4px;"> Pinding</h7>
                           <button class="btn p-0 delete" data-id='${row.orderId}'>
                           <i class="fa fa-trash-o delete" aria-hidden="true" style = "color: red; margin-left: -1px;" ></i>
                         </button>
                           `
             }},
            { 'data': 'dateString' },
            { 'data': 'totalPrice' },
        ],
  
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ordering": false,
        "info": true,
        "lengthChange": false,
        "paging": true,

        "drawCallback": function (settings) {
            debugger
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            var mySubGroup = null;
            $(rows).each(function () {
                var row = currentOrder.row($(this)).data();
                mySubGroup = row.dateString;
                if (last !== mySubGroup) {
                    $(this).before(
                        `<tr class="subgroup font-weight-bold" style="background-color:#E5E5E5;color:#168305;width:50px;" >
                              <td colspan="6" style="text-align:left;">${mySubGroup} </td>
                               <td><h7 style="color:#707070;margin-left: -68px;">Total Price:<h7>
                                 <h7 style="color:#168305;">${row.totalPrice}<h7>
                                 <button class="btn p-0 edit" style="margin-left: 13px;" data-id='${row.finalOrderId}' data-toggle="modal" data-target="#myModal">
                                   <i class="fa fa-pencil-square-o Update" aria-hidden="true" style="color: #A7CC3A;"></i>
                                     </button>
                                </td>
                              </tr>`
                    );

                    last = mySubGroup;

                }
            });
        }
    });

    //End of DataTabel Declarting
;
    //Delete Group Row
    $('#CurrentOrderTabel').on('click', 'tbody td .delete', function (e) {
        let Id = $(this).data('id');
        //console.log(Id);
        //let tr = $(this).parents('tr');
        //console.log(tr);
        //console.log(currentOrder);
        /* currentOrder.draw(false);*/
        Swal.fire({
            title: 'Do you want to Delete this Order?',
            showDenyButton: true,
            confirmButtonText: 'Yes',
            denyButtonText: `No`,
        }).then((result) => {
            if (result.isConfirmed) {
                //debugger
                //var rowId = Id;
                $.ajax({
                    url: '/Orders/DeleteOrderFromCurrentOrders',
                    data:
                    {
                        orderId: Id,
                    },
                    success: () => {
                        location.href = "https://localhost:44356/Orders/CurrentOrder";
                    }
                })
          
            }
            else if (result.isDenied) {
                Swal.fire('Changes are not saved', '', 'info')
            }
        })
      
    });

    $('#CurrentOrderTabel').on('click', 'tbody td .edit', function (e) {
        UpdatesOrdes = [];
        var finalOrderId = $(this).data('id');
        $.ajax({
            type: 'POST',
            url: '/Orders/GetOrdersBeEditedByPharmcy',
            data:
            {
                finalOrderId: finalOrderId,
            },
            success: function (result) {
                $("#tbodyid").empty();
                var TabelBody = "";
                for (let order of result) {
       
                    TabelBody += `<tr>
                                <td class='drugId' style="text-align: center;">${order.drugID}</td>
                                <td style="text-align: center;" class="DrugUpdate">
                                    <select class="choseDrug">
                                    <option selected>${order.drugName} </option>
                                    </select>
                                </td>
                                 <td style="text-align: center;"><input type="number" min="1" class="quantityUpdate" value="${order.quantity}" style="width: 50px;"/> </td>
                                <td class="price" style="text-align: center;">${order.pricePerUnit}</td>
                                 <td class="TotalPrice" style="text-align: center;">${order.totalPriceRow}</td>
                                 <td class="orderId" style="display:none; text-align: center;">${order.orderId}</td>
                                  <td class="finalOrderId" style="display:none; text-align: center;">${order.fInalOrderId}</td>
                                  </tr>`;

                }
                $('#editTabel').find('tbody').append(TabelBody);
                $(".choseDrug").select2({
                    placeholder: "Select or enter drug Name",
                    ajax: {
                        url: '/Orders/GetDrugs',
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

         
            }
        });
    });
/*  -------------------------------------------------  Update Button*---------------------------------------*/
   
    $('#UpdateButton').on('click', function (e) {
      
        debugger
        console.log(UpdatesOrdes);
        $.ajax({
            type: 'POST',
            url: '/Orders/UpdateOrdres',
            data: JSON.stringify(UpdatesOrdes),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                debugger
          
                if (result == 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Opss ,No Orders Updated ',
                        confirmButtonText: 'Ok',
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.href = "https://localhost:44356/Orders/CurrentOrder";
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
                            location.href = "https://localhost:44356/Orders/CurrentOrder";
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
                            location.href = "https://localhost:44356/Orders/CurrentOrder";
                        }
                    });
                }
            }
        });
    });
  
/* ---------------------------------------------- Edit Drug------------------------------------*/
    $('#editTabel').on("change", "tr td .choseDrug", function () {
        debugger
        var Quantity = $(this).closest("tr").find(".quantityUpdate").val();
        var tr = $(this).closest("tr");
        DrugIdUpdate = $(this).closest("tr").find(".choseDrug").val();
        $.ajax({

            url: '/Orders/GetspecificDrug',
            data:
            {
                DrugIdUpdate: DrugIdUpdate
            },
            success: function (result) {
                debugger
                var output = JSON.parse(result);
                if (output.quantityStorage < Quantity) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Opss ,New Quantity is Larger Than Qunatity Storge ',
                    });
                }
                else {
                    DrugUpdate = output.drugId;
                    Price = output.price;
                    tr.find(".drugId").text(DrugUpdate);
                    tr.find(".price").text(Price);
                    var total = Quantity * Price;
                  tr.find(".TotalPrice").text(total);
                }

            }
        });
    
    });
 /*------------------------------------------------------   Edit Quantity--------------------------------------*/

        $('#editTabel').on("change", "tr td .quantityUpdate", function () {
            debugger
            var quantityNew = $(this).val();
            if (quantityNew <= 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'please, Enter the Quantity grather than zero',
                });
            }
            else
            {
                var price = $(this).closest("tr").find(".price").text();
                var newTotalPrice = price * quantityNew;
                $(this).closest("tr").find(".TotalPrice").text(newTotalPrice);
                var finalOrderId = $(this).closest("tr").find(".finalOrderId").text();
                var orderId = $(this).closest("tr").find(".orderId").text();

                var DrugIdUpdate = $(this).closest("tr").find(".choseDrug").val();

                var UpdateOrderVM = {};

                UpdateOrderVM.drugId = DrugIdUpdate,
                    UpdateOrderVM.orderId = orderId,
                    UpdateOrderVM.QuaNew = quantityNew,
                    UpdateOrderVM.finalOrderId = finalOrderId
                UpdatesOrdes.push(UpdateOrderVM);
            }

          
        });
            

              
            
       
  
});
    

