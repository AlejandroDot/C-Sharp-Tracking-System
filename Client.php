<?php

//	Send beacon
if ($_GET["action"] === "beacon") {
    exec(("TrackingSystem ").urldecode($_GET["command"]).(" 2>&1"));
    exit; }

//	Render scheme
	else { ?>
    <!DOCTYPE html>
    <html>
        <head>
            <style>
                html, body { height: 100%; margin: 0; padding: 0; }
                #map { height: 100%; }
            </style>
        </head>
        <body>
            <div id="map"></div>
            <?php

            //	Render scheme
                if ($_GET["action"] === "scheme") { ?>
                <script>
                    function initMap() {
                        var map = new google.maps.Map(document.getElementById("map"), {
                            zoom: 17,
                            styles: [
                                {
                                    elementType: "geometry", 
                                    stylers: [{color: "#242f3e"}]
                                },
                                {
                                    elementType: "labels.text.stroke", 
                                    stylers: [{color: "#242f3e"}]
                                },
                                {
                                    elementType: "labels.text.fill", 
                                    stylers: [{color: "#746855"}]
                                },
                                {
                                    featureType: "administrative.locality",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "poi",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "poi.park",
                                    elementType: "geometry",
                                    stylers: [{color: "#263c3f"}]
                                },
                                {
                                    featureType: "poi.park",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#6b9a76"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "geometry",
                                    stylers: [{color: "#38414e"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "geometry.stroke",
                                    stylers: [{color: "#212a37"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#9ca5b3"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "geometry",
                                    stylers: [{color: "#746855"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "geometry.stroke",
                                    stylers: [{color: "#1f2835"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#f3d19c"}]
                                },
                                {
                                    featureType: "transit",
                                    elementType: "geometry",
                                    stylers: [{color: "#2f3948"}]
                                },
                                {
                                    featureType: "transit.station",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "geometry",
                                    stylers: [{color: "#17263c"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#515c6d"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "labels.text.stroke",
                                    stylers: [{color: "#17263c"}]
                                }
                            ],
                            <?php echo ("center: { lat: ").$_GET["entity"][1][latitude].(", lng: ").$_GET["entity"][1][longitude].(" },"); ?>
                            mapTypeId: "roadmap"
                        });
                        <?php

                            foreach ($_GET["entity"] as $metadata) {

                                if (isset($metadata["range"]) === true) {
                                    if ($metadata["range"] != 0) { echo ("new google.maps.Circle({ center: { lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" }, fillColor: \"#008757\", fillOpacity: 0.5, map: map, radius: ").$metadata[range].(", strokeColor: \"#008757\", strokeWeight: 2, title: \"").$metadata[identifier].("\"}); "); }
                                    echo ("new google.maps.Marker({ icon: { anchor: new google.maps.Point(24, 24), url: \"/assets/datapoint.png\" }, map: map, position: { lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" }, title: \"").$metadata[identifier].("\"}); "); }
                                else { echo ("new google.maps.Marker({ map: map, position: { lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" }, title: \"").$metadata[identifier].("\" }); "); } }

                        ?>
                    }
                </script>
                <script async defer src="https://maps.googleapis.com/maps/api/js?key=null&callback=initMap"></script>
            <?php }

            //	Trace route
                else if ($_GET["action"] === "trace") { ?>
                <script>
                    function initMap() {
                        var map = new google.maps.Map(document.getElementById("map"), {
                            zoom: 17,
                            styles: [
                                {
                                    elementType: "geometry", 
                                    stylers: [{color: "#242f3e"}]
                                },
                                {
                                    elementType: "labels.text.stroke", 
                                    stylers: [{color: "#242f3e"}]
                                },
                                {
                                    elementType: "labels.text.fill", 
                                    stylers: [{color: "#746855"}]
                                },
                                {
                                    featureType: "administrative.locality",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "poi",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "poi.park",
                                    elementType: "geometry",
                                    stylers: [{color: "#263c3f"}]
                                },
                                {
                                    featureType: "poi.park",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#6b9a76"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "geometry",
                                    stylers: [{color: "#38414e"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "geometry.stroke",
                                    stylers: [{color: "#212a37"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#9ca5b3"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "geometry",
                                    stylers: [{color: "#746855"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "geometry.stroke",
                                    stylers: [{color: "#1f2835"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#f3d19c"}]
                                },
                                {
                                    featureType: "transit",
                                    elementType: "geometry",
                                    stylers: [{color: "#2f3948"}]
                                },
                                {
                                    featureType: "transit.station",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "geometry",
                                    stylers: [{color: "#17263c"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#515c6d"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "labels.text.stroke",
                                    stylers: [{color: "#17263c"}]
                                }
                            ],
                            <?php echo ("center: { lat: ").$_GET["beacon"][1][latitude].(", lng: ").$_GET["beacon"][1][longitude].(" },"); ?>
                            mapTypeId: "roadmap"
                        });
                        <?php

                            foreach ($_GET["beacon"] as $beacon => $metadata) {
                            echo ("new google.maps.Marker({ label: \"").$beacon.("\", title: \"").$metadata[event].("\", map: map, position: { lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" } }); "); }
                            
                        ?>
                        var Coordinates = [
                            <?php

                                foreach ($_GET["beacon"] as $metadata) {
                                echo ("{ lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" }, "); }

                            ?>
                        ];
                        new google.maps.Polyline({
                            geodesic: true,
                            map: map,
                            path: Coordinates,
                            strokeColor: "#d85549",
                            strokeOpacity: 1,
                            strokeWeight: 2
                        });
                    }
                </script>
                <script async defer src="https://maps.googleapis.com/maps/api/js?key=null&callback=initMap"></script>
            <?php }

            //	Play scenario
                else if ($_GET["action"] === "scenario") { ?>
                <script>
                    function setLocation(position) {
                        navigator.geolocation.getCurrentPosition(initMap);
                    }
                    function updateLocation(position) {
                        people.setPosition(new google.maps.LatLng(position.coords.latitude, position.coords.longitude));
                        map.panTo(new google.maps.LatLng(position.coords.latitude, position.coords.longitude));
                    }
                    <?php
                    
                        if ($_GET["realtime"] === "true") {
                        echo "setInterval(function() { navigator.geolocation.getCurrentPosition(updateLocation); }, 2000);"; }
                    
                    ?>
                    function initMap(position) {
                        map = new google.maps.Map(document.getElementById("map"), {
                            zoom: 17,
                            styles: [
                                {
                                    elementType: "geometry", 
                                    stylers: [{color: "#242f3e"}]
                                },
                                {
                                    elementType: "labels.text.stroke", 
                                    stylers: [{color: "#242f3e"}]
                                },
                                {
                                    elementType: "labels.text.fill", 
                                    stylers: [{color: "#746855"}]
                                },
                                {
                                    featureType: "administrative.locality",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "poi",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "poi.park",
                                    elementType: "geometry",
                                    stylers: [{color: "#263c3f"}]
                                },
                                {
                                    featureType: "poi.park",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#6b9a76"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "geometry",
                                    stylers: [{color: "#38414e"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "geometry.stroke",
                                    stylers: [{color: "#212a37"}]
                                },
                                {
                                    featureType: "road",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#9ca5b3"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "geometry",
                                    stylers: [{color: "#746855"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "geometry.stroke",
                                    stylers: [{color: "#1f2835"}]
                                },
                                {
                                    featureType: "road.highway",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#f3d19c"}]
                                },
                                {
                                    featureType: "transit",
                                    elementType: "geometry",
                                    stylers: [{color: "#2f3948"}]
                                },
                                {
                                    featureType: "transit.station",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#d59563"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "geometry",
                                    stylers: [{color: "#17263c"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "labels.text.fill",
                                    stylers: [{color: "#515c6d"}]
                                },
                                {
                                    featureType: "water",
                                    elementType: "labels.text.stroke",
                                    stylers: [{color: "#17263c"}]
                                }
                            ],
                            center: { lat: position.coords.latitude, lng: position.coords.longitude },
                            mapTypeId: "roadmap"
                        });
                        <?php

                            foreach ($_GET["datapoint"] as $datapoint => $metadata) {
                                if ($metadata["range"] != 0) {
                                    echo ("datapoints.push(new google.maps.Circle({ center: { lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" }, fillColor: \"#008757\", fillOpacity: 0.5, map: map, radius: ").$metadata[range].(", strokeColor: \"#008757\", strokeWeight: 2, title: \"").$metadata[identifier].("\"})); "); 
                                    echo ("new google.maps.Marker({ icon: { anchor: new google.maps.Point(24, 24), url: \"/assets/datapoint.png\" }, map: map, position: { lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" }, title: \"").$metadata[identifier].("\"}); "); }
                                else if ($metadata["range"] == 0) { echo ("stations.push(new google.maps.Marker({ icon: { anchor: new google.maps.Point(24, 24), url: \"/assets/datapoint.png\" }, map: map, position: { lat: ").$metadata[latitude].(", lng: ").$metadata[longitude].(" }, title: \"").$metadata[identifier].("\"})); "); } }

                            foreach ($_GET["object"] as $metadata) {
                            echo ("objects.push(new google.maps.Marker({ draggable: true, label: \"O\", map: map, position: { lat: position.coords.latitude, lng: position.coords.longitude + 0.001 }, title: \"").$metadata[identifier].("\"})); "); }    

                            echo ("people = new google.maps.Marker({ draggable: true, label: \"P\", map: map, position: { lat: position.coords.latitude, lng: position.coords.longitude }, title: \"").$_GET["people"].("\" });");

                        ?>
                        function cmd(action, command) {
                            var xhttp = new XMLHttpRequest();
                            xhttp.open("GET", "/Client.php?action=" + action + "&command=" + encodeURI(command), true);
                            xhttp.send();
                            alert(command);
                        }
                        stations.forEach(function(station) {
                            google.maps.event.addListener(station, "click", function(event) {
                            cmd("beacon", "Beacon People " + station.title + " " + people.title); });
                        });
                        google.maps.event.addListener(people, "dragend", function(event) {
                            datapoints.forEach(function(datapoint) {
                                if (google.maps.geometry.spherical.computeDistanceBetween(people.getPosition(), datapoint.getCenter()) <= datapoint.getRadius()) {
                                cmd("beacon", "Beacon People " + people.title + " " + datapoint.title + " " + people.getPosition().lat() + " " + people.getPosition().lng()); }
                            });
                        });
                        objects.forEach(function(object) {
                            google.maps.event.addListener(object, "dragend", function(event) {
                                datapoints.forEach(function(datapoint) {
                                    if (google.maps.geometry.spherical.computeDistanceBetween(object.getPosition(), datapoint.getCenter()) <= datapoint.getRadius()) {
                                    cmd("beacon", "Beacon Object " + datapoint.title + " " + object.title + " " + object.getPosition().lat() + " " + object.getPosition().lng()); }
                                });
                            });
                        });
                    }
                    var map; var datapoints = []; var stations = []; var people; var objects = [];
                </script>
                <script async defer src="https://maps.googleapis.com/maps/api/js?key=null&callback=setLocation"></script>
            <?php } ?>
        </body>
    </html>
<?php } ?>