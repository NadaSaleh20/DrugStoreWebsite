
$(document).ready(function () {
    var curentOrderAdminVM =
    {
        drugId: 0,
        minPrice: 0,
        maxPrice: 0,
        pharmcyId: null,
        minDate: null,
        maxDate: null
    };

    /*--------------------------------------------------- Declartion DataTabel-------------------------------------*/
    var currentOrder = $('#ArcivedOrderAdmin').DataTable({
        "ajax": {

            url: '/Admin/GetArchivedOrderAdmin',
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
            targets: [0, 6, 7],
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
            {
                
                'data': function (data, type, row) {
                    if (data.orderStatus == 3) {   
                        return `<h7 class="statusMangefalied">Canceled</h7>`
                    }
                    else {
                        return `<h7 class="statusMange">Completed</h7>`
                    }
                },
               
            },
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
                if (colNo == 0) {
                    api.column(colNo, { page: 'current' }).data().each(function (group, i) {


                        if (last !== group) {
                            $(rows).eq(i).before(
                                `<tr class="group font-weight-bold "style="background-color: #F9F9F9;">
                                <td class="text-left  px-5" colspan="6" style="color: #707070;  font-family: Roboto;font-size: 15px;">
                                  ${api.column(0).data()[i]}</td>
                                      </tr>`

                            );
                            last = group;
                        }
                    });
                }
                else if (colNo == 6) {
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
                else {

                    api.column(colNo, { page: 'current' }).data().each(function (group, i) {

                        if (last !== group) {
                            $(rows).eq(i).before(
                                `<tr  style="background-color: #D4EDF9" >
                                  <td colspan="7" class="text-left  px-5" style="color: #707070; font-weight: bold;">Total Price: <span style="color: #168305;
                                        font-size: 15px;">${api.column(7).data()[i]}NIS</span></td>
                              </tr>`

                            );

                            last = group;
                        }
                    });
                }

            }
        },

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
        else {
            curentOrderAdminVM.minPrice = minPrice;
            curentOrderAdminVM.maxPrice = maxPrice;
            currentOrder.ajax.reload();
        }
        $('#FirstInput').val("");
        $('#SecondInput').val("");
    });
});
