
$(document).ready(function () {
    /*----------------------------------------------------Invoke Select2 For Add Drug-----------------------------------*/
    getSelect2($(".chose-drug"));
 /* ----------------------------------------------------------- DataTabel-Declartion-------------------------------------*/
    var t = $('#AddorderRow').DataTable({

        "columns": [
            { 'data': 'DrugID', "searchable": true },
            { 'data': 'DrugName', 'className': "MangeDName" },
            { "data": 'Quantity', 'className': "QuanUpdate" },
            { 'data': 'IsAvialable' },
            { 'data': 'pricePerUnit' },
            { 'data': 'TotalPriceRow' },
            {
                "title": "Edit",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, row, meta) {
                    return `
                          <button class="btn p-0 edit" data-category='+ row.Id + '>
                          <i class="fa fa-pencil-square-o Update" aria-hidden="true" style="color: #A7CC3A;"></i>
                         </button>
                         <button class="btn p-0 delete" data-id='+ row.Id + '>
                           <i class="fa fa-trash-o delete" aria-hidden="true" style = "color: red; margin-left: 10px;" ></i>
                         </button>`;
                }
            }
        ],
        "processing": false,
        "serverSide": false,
        "filter": false,
        "orderMulti": false,
        "ordering": false,
        "info": false,
        "lengthChange": false,
        "bPaginate": false,
        "searching": true

    });

/*  ----------------------------------------------------select2 function-----------------------------------------*/
    function getSelect2(value) {
        debugger
        value.select2({
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

    /*------------------------------------------Function IsAvalible CheackBox Styling--------------------------*/
    function isAvalibleCheckBox(value, rowNode) {
        if (!value) {
            $(rowNode).css('background-color', '#F9E4E3');
            $(rowNode).find("td:eq(3)").html('<i class="fa fa-check-square" aria-hidden="true" style="color:#FF5810"></i>UnAvaliable');
        }
        else {
            $(rowNode).find("td:eq(3)").html('<i class="fa fa-check-square" aria-hidden="true" style="color:#168305"></i>Avaliable');
        }
    }
    /*------------------------------------------ functionIsExist------------------------*/
    function IsExist(DrugText) {
        //Make Sure that the Tabel  haven't  The drug you want to add yet !
        var DrugsName = t.column(1).data().toArray();
        var IsExisit = jQuery.inArray(DrugText, DrugsName);
        if (IsExisit >= 0) {
            return true;
        }
    }
 /* -----------------------------------------function Add Or Update--------------------------------------------*/


    function createOrderInfo(DrugId, DrugText, DrugQuantity, AddOrUpdate, tr) {
        debugger
        //Make sure That the quantity and select2 have value befor Added it 
        if (DrugQuantity == null || DrugId == null) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'please, Enter the Quantity on Select2 first',
            });
            return;
        }

            //Send them with Ajax Request
        $.ajax({
            url: '/Orders/GetRowDrugInfo',
            data: {
                drugId: DrugId,
                Qunantity: DrugQuantity
            },

            success: function (result) {
                var valueofCheckBox = result.isAvialable;
                if (AddOrUpdate == 1) {
                    var isExiest = IsExist(DrugText);
                    if (!isExiest) {
                        var rowNode = t.row.add({
                            "DrugID": result.drugID,
                            "DrugName": result.drugName,
                            "Quantity": result.quantity,
                            "IsAvialable": result.isAvialable,
                            "pricePerUnit": result.pricePerUnit,
                            "TotalPriceRow": result.totalPriceRow
                        }).draw().node();

                        isAvalibleCheckBox(valueofCheckBox, rowNode);
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You Have already added this drug! , You Can edit or delete it',
                        });
                    }
             
                }
                else {
                    t.row(tr).remove().draw(false);
                    var isExiest = IsExist(DrugText);
                    if (!isExiest) {
                        var rowNode = t.row.add({
                            "DrugID": result.drugID,
                            "DrugName": result.drugName,
                            "Quantity": result.quantity,
                            "IsAvialable": result.isAvialable,
                            "pricePerUnit": result.pricePerUnit,
                            "TotalPriceRow": result.totalPriceRow
                        }).draw().node();
                        isAvalibleCheckBox(valueofCheckBox, rowNode);
                    }
                    else
                    {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You Have already added this drug! , You Can edit or delete it',
                        });
                    }
        
                }
 
            }
                
            });
    }

 /*---------------------------------------------- Add Button Click----------------------------------------------------*/
    $("#AddButton").on("click", function () {
        //Get DrugId ,DrugName , DrugQunatity
        var DrugId = $(".chose-drug").find(":selected").val();
        var DrugText = $(".chose-drug").find(":selected").text();
        var DrugQuantity = $("#DrugQun").val();
        var AddOrUpdate = 1;
        createOrderInfo(DrugId, DrugText, DrugQuantity, AddOrUpdate);
        $("#DrugQun").val('');
        $(".chose-drug").empty().trigger('change');

    });

 /* ------------------------------------------------Pressing Delete Icon------------------------------------- */ 
    $('#AddorderRow').on('click', 'tbody td .delete', function (e) {
        Swal.fire({
            title: 'Do you want to Delete this Order?',
            showDenyButton: true,
            confirmButtonText: 'Yes',
            denyButtonText: `No`,
        }).then((result) => {
            if (result.isConfirmed) {
                t.row($(this).parents('tr')).remove().draw(false);
            
            }
            else if (result.isDenied) {
                Swal.fire('Changes are not saved', '', 'info')
            }
        })
    });

  - /* ---------------------------------------------------Edit Row------------------------------------------------*/
    $('#AddorderRow').on('click', 'tbody td .Update', function (e) {
        debugger
        //We Want to edit DrugName

        var QuantityValue = $(this).closest("tr").find(".QuanUpdate").text();
        var DrugValue = $(this).closest("tr").find(".MangeDName").text();
        t.row($(this).parents('tr').find("td:eq(1)").html(`<select class="Update-drug"><option selected>${DrugValue} </option></select>`));
        getSelect2($(".Update-drug"));
       
        //We Want to edit Quantity
        t.row($(this).parents("tr").find("td:eq(2)").html('<input id="editQua" type="number" min="1" style="width: 60px;"/>'));
        $('#editQua').val(QuantityValue);
        //Add Update Button Insted of icon
        t.row($(this).parents('tr').find("td:eq(6)").html('<button id="editButton">Update</button>'));

        //When press on editButton
        $('#editButton').on('click', function (e) {
            //Get DrugId ,DrugName , DrugQunatity
            var DrugId = $(".Update-drug").find(":selected").val();
            var DrugText = $(".Update-drug").find(":selected").text();
            var DrugQuantity = $("#editQua").val();
            var AddOrUpdate = 2;
            let tr = $(this).parents("tr");
            createOrderInfo(DrugId, DrugText, DrugQuantity, AddOrUpdate, tr);
        });
  
    });

- /* ------ -------------------------------------------------Save in DB---------------------------------------------------*/
    $("#saveButton").on("click", function () {
       //Get List of data Storted in dataTabel
        var rows = t.rows().data().toArray();
        var dataList = {
            DrugInfoObjectRow: rows
        };
        
       // if no order founded in the tabel
        if (rows.length == 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'There is No Order to Save it',
            })
            $("#DrugQun").val('');
            $("#chose-drug").val('').trigger('change');
            t.clear().draw();
        }

        else {
            $.ajax({
                type: "POST",
                data: JSON.stringify(dataList),
                url: '/Orders/saveOrder',
                contentType: "application/json",
                success: () => {
                    location.href = "https://localhost:44356/Orders/CurrentOrder";
                }
                
            });
        }
    });

});







