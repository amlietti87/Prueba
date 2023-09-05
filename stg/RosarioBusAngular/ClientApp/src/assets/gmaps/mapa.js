


            var map = null;
            var geocoder = null;
            var directionsService = null;
            var info;
            var marker;

            function init() {

                // Some basic map setup (from the API docs)
                var mapOptions = {
                    center: new google.maps.LatLng(-33.00, -60.78),
                    zoom: 13,
                    mapTypeControl: false,
                    streetViewControl: false,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                map = new google.maps.Map(document.getElementById('map'), mapOptions);
                info = document.getElementById('info');

                geocoder = new google.maps.Geocoder();

            }

            //////// muliple autoroute multiple waypoints, multiple service //////////////
            var routeService = "GOOGLE"  //or OSM_YOURS
            var routeMode = "cycle"  //bicycle, foot, ,motorcar //Google = BICYCLING
            var routeMarkers = [];  // for route legs/steps
            var routeMarkersArray = []; //keep all routemarkers of differnt routes
            var routeLinesArray = []; //keep all routeLines of differnt routes

            var setRM = 0;

            function setRouteMarker() {
                if (setRM == 0) {     //start a new route
                    prop = document.getElementById("sRoute");
                    prop.style.color = "red";
                    prop.style.fontWeight = "bold";
                    temp = google.maps.event.addListener(map, 'click', function (event) {
                        addMarker(event.latLng, true)
                    });
                    directionsService = new google.maps.DirectionsService();
                    map.setOptions({ draggableCursor: 'crosshair' });
                    setRM = 1;
                } else {       //finish a route
                    prop = document.getElementById("sRoute");
                    prop.style.color = "black";
                    prop.style.fontWeight = "normal";
                    map.setOptions({ draggableCursor: null });
                    google.maps.event.removeListener(temp);
                    routeMarkersArray.push(routeMarkers)
                    routeMarkers = [];
                    routeLinesArray.push(routeLines)
                    routeLinesArray = [];
                    var routeKM = 0;
                    setRM = 0;
                }
            }

            function dibugarRoute(myLatLngp) {
               
                myLatLng=[];
                 
                //if (myLatLngp == null) {
                    //myLatLngp= [];
                    //myLatLngp.push({ lng: -60.786915, lat: -33.005444 });
                    //myLatLngp.push({ lng: -60.787388, lat: -33.004842 });
                    //myLatLngp.push({ lng: -60.779841, lat: -33.001023 });
                    //myLatLngp.push({ lng: -60.776431, lat: -32.999291 });
                    //myLatLngp.push({ lng: -60.775529, lat: -32.998865 });
                    //myLatLngp.push({ lng: -60.775781, lat: -32.998411 });
                    //myLatLngp.push({ lng: -60.774335, lat: -32.997749 });
                //}
                    
                   
                
                for (var i = 0; i < myLatLngp.length-1; i++) { 

                    myLatLng.push({ lng: myLatLngp[i].lng , lat:  myLatLngp[i].lat});
                     
                }





                for (var i = 0; i < myLatLng.length - 1; i++) {
                    //tempMarker = new google.maps.Marker({
                    //    position: myLatLng[i],
                    //    icon: pinYellow,
                    //    map: map,
                    //    title: "Remove to insert Route",
                    //    draggable: true
                    //});

                    addMarker(myLatLng[i]);

                }
            }


            var pinRed = {
                url: 'http://maps.gstatic.com/mapfiles/ridefinder-images/mm_20_red.png',
            };
            var pinYellow = {
                url: 'http://maps.gstatic.com/mapfiles/ridefinder-images/mm_20_yellow.png',
            };




            // OSM YOURS DIRECTION Service//
            function addMarker(latlng) {
                /*
                geocoder.geocode({'location': latlng}, function(results, status) {
                    if (status === google.maps.GeocoderStatus.OK) {
                        mTitle = results[1].formatted_address;
                    } else {
                        mTitle = latlng.toUrlValue(6);
                    }
                });
                */
                mTitle = "wp: " + routeMarkers.length;
                marker = new google.maps.Marker({
                    position: latlng,
                    icon: pinRed,
                    map: map,
                    //title: mTitle,  //latlng.toUrlValue(6),
                    draggable: true
                })
                marker.uid = routeMarkers.length;
                addRouteMarkerListener();
                routeMarkers.push(marker);

                routeService = document.getElementById("rtService").value;
                routeMode = document.getElementById("rtMode").value;
                if (routeMarkers.length > 1) {
                    if (routeService == "OSM_YOURS") { calcYoursRoute() };
                    if (routeService == "GOOGLE") { calcGoogleRoute() };
                }
            }

            function addRouteMarkerListener() {
                google.maps.event.addListener(marker, 'dragend', function (e) {
                    uid = this.uid;
                    console.log("wp:" + uid);
                    if (uid > 0) {   //move route for middle and last marker
                        console.log("Route1")
                        moveRoute(uid - 1);   // call sube for calc
                        if (uid < routeMarkers.length - 1) {
                            moveRoute(uid);
                        }
                    }

                    //move route for first point
                    if (uid == 0) {
                        routeLine = routeLines[0];
                        routeKM = routeKM - routeLine.km
                        routeLine.setMap(null);
                        if (routeService == "OSM_YOURS") { calcYoursRoute(1) };
                        if (routeService == "GOOGLE") { calcGoogleRoute(1) };

                    }
                }); //< dragend
                google.maps.event.addListener(marker, 'rightclick', function (e) {
                    uid = this.uid;
                    console.log("remove wp:" + uid);
                    // first Marker (uid), del first route (uid)
                    if (uid == 0) {
                        routeMarkers[uid].setMap(null);
                        routeMarkers.splice(uid, 1); //delete first element

                        routeLine = routeLines[uid];
                        routeKM = routeKM - routeLine.km
                        routeLine.setMap(null);
                        routeLines.splice(uid, 1);
                        if (routeLines.length == 1) {
                            routeLines = [];
                            routeKM = 0;
                            routeMarker.uid = 0;
                            info.innerHTML = routeKM + " km / " + routeMarkers.length + " wpts";
                        } else {
                            routeLines.splice(uid - 1, 1);
                            renumber()
                        }

                    }
                    // last marker (uid), del last route (uid - 1)
                    if (uid == routeMarkers.length - 1) {
                        routeMarkers[uid].setMap(null);
                        routeMarkers.splice(uid, 1); //delete first element

                        routeLine = routeLines[uid - 1];
                        routeKM = routeKM - routeLine.km
                        routeLine.setMap(null);
                        if (routeLines.length == 1) {
                            routeLines = [];
                            routeKM = 0;
                            info.innerHTML = routeKM + " km / " + routeMarkers.length + " wpts";
                        } else {
                            routeLines.splice(uid - 1, 1);
                        }
                    }
                    //  middle Marker (uid), del route before and after (uid - 1), calc new route
                    if (uid > 0 && uid < routeMarkers.length - 1) {
                        routeMarkers[uid].setMap(null);
                        routeMarkers.splice(uid, 1); //delete first element
                        //entfernt nachfolgende Route
                        console.log("Del nachfolgende Route: " + uid + " von gesamt Routen: " + routeLines.length)
                        routeLine = routeLines[uid];
                        routeKM = routeKM - routeLine.km
                        routeLine.setMap(null);
                        routeLines.splice(uid, 1);

                        renumber();
                        console.log("Del/Replace vorhergehende Route: " + (uid - 1) + " von gesamt Routen: " + routeLines.length);
                        routeLine = routeLines[uid - 1];
                        routeKM = routeKM - routeLine.km
                        routeLine.setMap(null);

                        console.log("calc neue Route: " + (uid - 1));
                        if (routeService == "OSM_YOURS") { calcYoursRoute(uid) };
                        if (routeService == "GOOGLE") { calcGoogleRoute(uid) };


                    }

                }); //< rightclick
            } //< end routeMarkerListener

            function renumber() {

                console.log("renumber");
                for (i = 0; i < routeMarkers.length; i++) {
                    console.log("renumber Marker:" + i);
                    routeMarkers[i].uid = i;
                    routeMarkers[i].setTitle = "wp: " + i;

                }
                for (i = 0; i < routeLines.length; i++) {
                    console.log("renumber Lines: " + i);
                    routeLine = routeLines[i]
                    routeLines.uid = i;
                }
            }

            function moveRoute(uid) {  //sub for dragend routemarker
                console.log("Route2");	//routline after Marker
                routeLine = routeLines[uid];
                routeKM = routeKM - routeLine.km;
                routeLine.setMap(null);
                if (routeService == "OSM_YOURS") { calcYoursRoute(uid + 1) };
                if (routeService == "GOOGLE") { calcGoogleRoute(uid + 1) };

            }

            function wait(ms) {   //not in use
                var start = new Date().getTime();
                var end = start;
                while (end < start + ms) {
                    end = new Date().getTime();
                }
            }

            function calcYoursRoute(index) {

                str = 'http://www.yournavigation.org/api/1.0/gosmore.php?format=kml&'
                    + 'flat={startLat}&flon={startLon}&tlat={endLat}&tlon={endLon}&v={travelMode}&fast=1&layer=mapnik'

                if (typeof (index) == 'undefined') {
                    index = "new";
                    start = routeMarkers[routeMarkers.length - 2].getPosition().toUrlValue(6);;
                    end = routeMarkers[routeMarkers.length - 1].getPosition().toUrlValue(6);
                } else {
                    start = routeMarkers[index - 1].getPosition().toUrlValue(6);
                    end = routeMarkers[index - 0].getPosition().toUrlValue(6);
                }

                s = start.split(",");
                e = end.split(",");
                startLat = s[0]; startLon = s[1];
                endLat = e[0]; endLon = e[1];
                //alert(endLon);
                str = str.replace("{startLat}", startLat);
                str = str.replace("{startLon}", startLon);
                str = str.replace("{endLat}", endLat);
                str = str.replace("{endLon}", endLon);
                // set travelMode //
                str = str.replace("{travelMode}", "bicycle"); //cycle = Standard
                if (routeMode == "drive") { str = str.replace("{travelMode}", "motorcar") };
                if (routeMode == "walk") { str = str.replace("{travelMode}", "foot") };

                //console.log(str);
                data = httpGet(str);

                coord = data.split("<coordinates>")
                coord = coord[1].split("</coordinates>")
                coord = coord[0].trim();
                //console.log(coord);
                var path = [];
                var clines = coord.split("\n");
                for (i = 0; i < clines.length; i++) {
                    iArray = clines[i].split(",");
                    lng = iArray[0];
                    lat = iArray[1];

                    point = new google.maps.LatLng(lat, lng);
                    path.push(point)
                }

                //<distance>21.452372</distance>
                //<traveltime>1228</traveltime>
                // Get distance (in KM)
                dist = data.split("<distance>")
                dist = dist[1].split("</distance>")
                dist = dist[0].trim() * 1;
                dist = dist.toFixed(3) * 1;
                //write the route
                writeRoute(path, dist, index);
            }

            function httpGet(theUrl) {
                var xhttp = new XMLHttpRequest();
                xhttp.open("GET", theUrl, false); //asyncron = true doesn't work correct.
                xhttp.send();
                result = xhttp.responseText;
                info.innerHTML = result;
                return result;
                //Notes from:http://www.w3schools.com/ajax/ajax_xmlhttprequest_send.asp
                //Using async=false is not recommended, but for a few small requests this can be ok.
                //Remember that the JavaScript will NOT continue to execute, until the server response is ready.
                //If the server is busy or slow, the application will hang or stop.
                //Note: When you use async=false, do NOT write an onreadystatechange function -
                // just put the code after the send() statement:
            }



            function calcGoogleRoute(index) {

                if (typeof (index) == 'undefined') {
                    index = "new";
                    var rStart = routeMarkers[routeMarkers.length - 2].getPosition();
                    var rEnd = routeMarkers[routeMarkers.length - 1].getPosition();
                } else {
                    var rStart = routeMarkers[index - 1].getPosition();
                    var rEnd = routeMarkers[index - 0].getPosition();

                }
                request = {
                    origin: rStart,
                    destination: rEnd
                };

                if (routeMode == "cycle") { request.travelMode = google.maps.DirectionsTravelMode.BICYCLING };
                if (routeMode == "drive") { request.travelMode = google.maps.DirectionsTravelMode.DRIVING };
                if (routeMode == "walk") { request.travelMode = google.maps.DirectionsTravelMode.WALKING };

                directionsService.route(request, function (result, status) {
                    if (status == google.maps.DirectionsStatus.OK) {
                        path = result.routes[0].overview_path
                        var sumKM = 0;
                        var sumTime = 0;
                        var myroute = result.routes[0];
                        for (var i = 0; i < myroute.legs.length; i++) {
                            sumKM += myroute.legs[i].distance.value;
                            sumTime += myroute.legs[i].duration.value;
                        }
                        sumKM = sumKM / 1000;
                        console.log("Route calculated, write the route")
                        writeRoute(path, sumKM, index, sumTime)
                    } else if (status == google.maps.DirectionsStatus.ZERO_RESULTS) {
                        alert("Could not find a route between these points");
                    } else {
                        alert("Directions request failed");
                    }
                });
            }

            var routeLine = null;
            var routeLines = [];
            var routeKM = 0;
            var routeTime = 0;

            function writeRoute(path, routekm, index, routetime) {
                console.log("Write Route: " + (index - 1));
                color = "grey"
                if (routeService == "GOOGLE") { color = 'blue' };
                if (routeService == "OSM_YOURS") { color = 'grey' };
                var polyOptions = {
                    map: map,
                    path: path,
                    strokeColor: color,
                    strokeOpacity: 1.0,
                    strokeWeight: 3,
                    editable: false
                };
                routeLine = new google.maps.Polyline(polyOptions);
                routeLine.km = routekm;
                routeLine.service = routeService;
                routeLine.mode = routeMode;
                routeLine.uid = routeLines.length;
                addRouteLineListener();
                if (index == "new") {
                    console.log("new route:" + (routeLines.length));
                    routeLines.push(routeLine);
                } else {
                    console.log("replace route:" + (index - 1));
                    routeLine.uid = index - 1;
                    routeLines[index - 1] = routeLine;

                }
                routeKM += routekm;
                routeTime += routetime;
                info.innerHTML = routeKM + " km / " + routeMarkers.length + " wpts / " + routeTime + " time";
            }

            var tempMarker = null;
            function addRouteLineListener() {

                google.maps.event.addListener(routeLine, 'mouseover', function () {
                    console.log("routeLine id = " + this.uid)
                });

                google.maps.event.addListener(routeLine, 'click', function (event) {
                    //set temp marker and save route line id
                    lid = this.uid
                    latlng = event.latLng
                   
                    if (tempMarker) tempMarker.setMap(null);
                    tempMarker = new google.maps.Marker({
                        position: latlng,
                        icon: pinYellow,
                        map: map,
                        title: "Remove to insert Route",
                        draggable: true
                    })
                    tempMarker.lid = lid;

                    // set listener for temp marker
                    // rightclick del tempmarker
                    google.maps.event.addListener(tempMarker, 'rightclick', function () {
                        tempMarker.setMap(null);
                    })
                    //dragend replace temmarker und calc inserted routes
                    google.maps.event.addListener(tempMarker, 'dragend', function (e) {
                        //replace marker
                        lid = this.lid
                        latlng = e.latLng
                        if (tempMarker) tempMarker.setMap(null);
                        marker = new google.maps.Marker({
                            position: latlng,
                            icon: pinRed,
                            map: map,
                            //title: mTitle,  //latlng.toUrlValue(6),
                            draggable: true
                        })
                        marker.uid = lid + 1;
                        addRouteMarkerListener();
                        routeMarkers.splice(lid + 1, 0, marker);
                        renumber();
                        // write first route
                        routeLine = routeLines[lid];
                        routeKM = routeKM - routeLine.km
                        routeLine.setMap(null);

                        routeService = document.getElementById("rtService").value;
                        routeMode = document.getElementById("rtMode").value;

                        if (routeService == "OSM_YOURS") { calcYoursRoute(lid + 1) };
                        if (routeService == "GOOGLE") { calcGoogleRoute(lid + 1) };

                        // write second route
                        routeLines.splice(lid + 1, 0, routeLines[lid]); //insert new item
                        renumber();
                        if (routeService == "OSM_YOURS") { calcYoursRoute(lid + 2) };
                        if (routeService == "GOOGLE") { calcGoogleRoute(lid + 2) };
                        console.log("routlines: " + routeLines.length);

                    }) //> end temp marker dragend
                }); //> end routeline on click
            } //> end routeline listener


            function resetRoute() {
                for (var i in routeLines) {
                    routeLines[i].setMap(null);
                }
                routeLines = [];

                for (var i in routeMarkers) {
                    routeMarkers[i].setMap(null);
                }
                routeMarkers = [];
                routeKM = 0;
            }


            // Get the ball rolling and trigger our init() on 'load'
            google.maps.event.addDomListener(window, 'load', init);

        