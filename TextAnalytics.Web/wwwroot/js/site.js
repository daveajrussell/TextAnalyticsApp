function getClientData(clientId, clientName) {
    if (clientId > 0) {

        $.ajax(`/api/clients/getclientdata/${clientId}`).done(function (model) {
            Highcharts.chart('container', {
                title: {
                    text: 'Client tweet sentiment'
                },
                yAxis: {
                    title: {
                        text: 'Sentiment'
                    },
                    min: 0.0,
                    max: 1.0
                },
                xAxis: {
                    categories: model.categories
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                series: model.series
            });
        });

        return window.history.pushState('', `${clientName} Tweet Analysis`, `/clients/${clientId}`);
    } else {
        $.ajax('/api/clients/getclientdata').done(function (model) {
            Highcharts.chart('container', {

                title: {
                    text: 'Client tweet sentiment'
                },
                yAxis: {
                    title: {
                        text: 'Sentiment'
                    },
                    min: 0.0,
                    max: 1.0
                },
                xAxis: {
                    categories: model.categories
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                series: model.series
            });
        });

        return window.history.pushState('', 'Tweet Analysis', '/');
    }
}

$(function () {

    window.addEventListener('popstate', function (event) {
        if (window.location.pathname !== '/') {
            var id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
            $('option:selected').val(id);
        }

        var $selected = $('option:selected');

        var clientId = parseInt($selected.val()),
            clientName = $selected.text();

        getClientData(clientId, clientName);
    });

    var $selected = $('option:selected');

    var clientId = parseInt($selected.val()),
        clientName = $selected.text();

    getClientData(clientId, clientName);


    $('#sel-client').on('change',
        function () {
            var $selected = $('option:selected');

            var clientId = parseInt($selected.val()),
                clientName = $selected.text();

            getClientData(clientId, clientName);
        });
});