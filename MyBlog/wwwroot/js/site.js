$(document).ready(
    function () {
        loadWeather();
    }
);

function loadWeather() {
    
    var xhr = new XMLHttpRequest();
    //var place = id
    xhr.open("GET", "https://" + weatherHost + "/data/2.5/weather?q=sydney&appid=" + weatherAPIKey + "&units=metric", true);
    xhr.onreadystatechange = function (e) {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                var response = JSON.parse(xhr.responseText);
                var temperature = parseInt(response.main.temp);
                var wtDescription = response.weather[0].description;
                                
                document.getElementById('weather').innerHTML = `<div class="wt"><i class="fas fa-cloud-sun"></i><span class="wtSummary">` + wtDescription + `</span> ` + Math.round(temperature) + `°C </div>`;
            } else {
                console.error(xhr.statusText);
            }
        }
    };
    xhr.send(null);
}


