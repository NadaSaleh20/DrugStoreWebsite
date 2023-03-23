$(document).ready(function (){
    //Coulmn Index
    var groupColumn = 7;
    //Delcaring dataTabel 
    var currentOrder = $('#ArcviedOrderTabel').DataTable({

        //hide coulmn TotalPrice And Day
        columnDefs: [
            { visible: false, targets: groupColumn, type: 'date' },
        ],
        "ajax": {
            "url": '/Orders/GetArcviedOrder',
            "dataSrc": ""
        },
        "columns": [
            { 'data': 'drugID', "searchable": true },
            { 'data': 'drugName', 'className': "MangeDName" },
            { "data": 'quantity' },
            {
                'data': 'isAvialable',
                'render': function (data, type, row) {
                    return '<i class="fa fa-check-square" aria-hidden="true" style="color:#168305"></i>Avaliable';
                }
            },
            { 'data': 'pricePerUnit' },
            { 'data': 'totalPriceRow' },
            {
                'data': function (data, type, row) {
                    debugger
                    if (data.status == 3) {
                        return 'Canseled';
                    }
                    else {
                        return 'Completed';
                    }
                },
                'className' : 'statusMange'
            },
            { 'data': 'dateString' },
        
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
                              <td colspan="8" style="text-align:left;">${mySubGroup} </td>
                              </tr>`
                    );

                    last = mySubGroup;

                }
            });
        }
    });




});
