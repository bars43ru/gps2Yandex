
function routeList() {
    fetch('api/route/list', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
        body: null
    })
        .then(response => response.json())
        .then(data => resultBind(data))
        .catch(error => console.error('Unable to get list route.', error));
}

function transportList() {
    jsonResponse = document.getElementById('json-text');

    fetch('api/transport/list', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
        body: null
    })
        .then(response => response.json())
        .then(data => resultBind(data))
        .catch(error => console.error('Unable to get list transport.', error));
}

function scheduleList() {
    jsonResponse = document.getElementById('json-text');

    fetch('api/schedule/list', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
        body: null
    })
        .then(response => response.json())
        .then(data => resultBind(data))
        .catch(error => console.error('Unable to get list transport.', error));
}

function resultBind(data) {

    jsonResponse = document.getElementById('json-text');
    jsonResponse.innerText = JSON.stringify(data, undefined, 4);

    divView = document.getElementById('div-table');

    if (data.length === 0) {
        divView.innerText = "";
        return;
    }

    var result = "<table border='1'><tr>";
    for (var prop in data[0]) {
        result += "<th>" + prop + "</th>";
    }
    result += "</tr>";

    for (var index in data) {
        result += "<tr>";
        for (var prop in data[index]) {
            result += "<td>" + data[index][prop] + "</td>";
        }
        result += "</tr>";
    }

    result += "</table>";


    divView.innerHTML = result;
}