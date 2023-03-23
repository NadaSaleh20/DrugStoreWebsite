/*/*-------------------------------------- DataTabel Declaration*-----------------------------------/*/
var t = $('#CustomerName').DataTable({
"ajax": {
        "url": '/Admin/GetPharmcies',
        "dataSrc": ""
    },
    "columns": [
        { 'data': 'accountNumber', "searchable": true, 'className':'tabelCostomer' },
        { 'data': 'pharmcyName', 'className': 'tabelCostomer'  },
    ],
    "processing": true,
    "serverSide": true,
    "filter": false,
    "orderMulti": false,
    "ordering": false,
    "info": false,
    "lengthChange": false,
    "paging": true,
});

$('.pagination').css('margin-right', '554px');