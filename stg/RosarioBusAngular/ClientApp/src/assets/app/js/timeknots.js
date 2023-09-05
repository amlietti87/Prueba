var TimeKnots = {
    draw: function (id, events, options) {
        var cfg = {
            width: null,
            height: 200,
            radius: 10,
            lineWidth: 4,
            color: "#999",
            background: "#FFF",
            dateFormat: "%Y/%m/%d %H:%M:%S",
            horizontalLayout: true,
            showLabels: false,
            labelFormat: "%Y/%m/%d %H:%M:%S",
            addNow: false,
            seriesColor: d3.schemeCategory10,
            dateDimension: true
        };


        //default configuration overrid
        if (options != undefined) {
            for (var i in options) {
                cfg[i] = options[i];
            }
        }
        if (cfg.addNow != false) {
            events.push({ date: new Date(), name: cfg.addNowLabel || "Today" });
        }


        if (events.length > 0 && !cfg.width) {
            cfg.width = events.length * 80;
        }

        var tip = d3.select(id)
            .append('div')
            .style("opacity", 0)
            .style("position", "absolute")
            .style("font-family", "Helvetica Neue")
            .style("font-weight", "300")
            .style("background", "rgba(0,0,0,0.5)")
            .style("color", "white")
            .style("padding", "5px 10px 5px 10px")
            .style("-moz-border-radius", "8px 8px")
            .style("border-radius", "8px 8px");
        var svg = d3.select(id).append('svg').attr("width", cfg.width + 80).attr("height", cfg.height);




        //Calculate times in terms of timestamps
        if (!cfg.dateDimension) {
            var timestamps = events.map(function (d) { return d.value });//new Date(d.date).getTime()});
            var maxValue = d3.max(timestamps);
            var minValue = d3.min(timestamps);
        } else {
            var timestamps = events.map(function (d) { return Date.parse(d.date); });//new Date(d.date).getTime()});
            var maxValue = d3.max(timestamps);
            var minValue = d3.min(timestamps);
        }
        var margin = (d3.max(events.map(function (d) { return d.radius })) || cfg.radius) * 1.5 + cfg.lineWidth;
        var step = (cfg.horizontalLayout) ? ((cfg.width - 2 * margin) / (maxValue - minValue)) : ((cfg.height - 2 * margin) / (maxValue - minValue));
        var series = [];
        if (maxValue == minValue) { step = 0; if (cfg.horizontalLayout) { margin = cfg.width / 2 } else { margin = cfg.height / 2 } }

        linePrevious = {
            x1: null,
            x2: null,
            y1: null,
            y2: null
        }

        //defs = svg.append("defs")

        //defs.append("marker")
        //    .attr({
        //        "id": "arrow",
        //        "viewBox": "0 -5 10 10",
        //        "refX": 5,
        //        "refY": 0,
        //        "markerWidth": 4,
        //        "markerHeight": 4,
        //        "orient": "auto"
        //    })
        //    .append("path")
        //    .attr("d", "M0,-5L10,0L0,5")
        //    .attr("class", "arrowHead");


        svg.selectAll("line")
            .data(events).enter().append("line")
            .attr("class", "timeline-line arrow")
            //.attr("marker-end", "url(#arrow)")
            .attr("x1", function (d) {
                var ret;
                if (cfg.horizontalLayout) {
                    var datum = (cfg.dateDimension) ? new Date(d.date).getTime() : d.value;
                    ret = Math.floor(step * (datum - minValue) + margin)
                }
                else {
                    ret = Math.floor(cfg.width / 2)
                }
                linePrevious.x1 = ret
                return ret
            })
            .attr("x2", function (d) {
                if (linePrevious.x1 != null) {
                    return linePrevious.x1
                }
                if (cfg.horizontalLayout) {
                    var datum = (cfg.dateDimension) ? new Date(d.date).getTime() : d.value;
                    ret = Math.floor(step * (datum - minValue))
                }
                return Math.floor(cfg.width / 2)
            })
            .attr("y1", function (d) {
                var ret;
                if (cfg.horizontalLayout) {
                    ret = Math.floor(cfg.height / 2)
                }
                else {
                    var datum = (cfg.dateDimension) ? new Date(d.date).getTime() : d.value;
                    ret = Math.floor(step * (datum - minValue)) + margin
                }
                linePrevious.y1 = ret
                return ret
            })
            .attr("y2", function (d) {
                if (linePrevious.y1 != null) {
                    return linePrevious.y1
                }
                if (cfg.horizontalLayout) {
                    return Math.floor(cfg.height / 2)
                }
                var datum = (cfg.dateDimension) ? new Date(d.date).getTime() : d.value;
                return Math.floor(step * (datum - minValue))
            })
            .style("stroke", function (d) {


                if (d.linecolor != undefined) {
                    return d.linecolor
                }
                if (d.color != undefined) {
                    return d.color
                }
                if (d.series != undefined) {
                    if (series.indexOf(d.series) < 0) {
                        series.push(d.series);
                    }
                    return cfg.seriesColor[series.indexOf(d.series)];
                }
                return cfg.color
            })
            .style("stroke-width", cfg.lineWidth);

        svg.selectAll("circle")
            .data(events).enter()
            .append("circle")
            .attr("class", "timeline-event") 
            .attr("r", function (d) { if (d.radius != undefined) { return d.radius } return cfg.radius })
            .style("stroke", function (d) {

                if (d.stroke != undefined) {
                    return d.stroke
                }
                if (d.color != undefined) {
                    return d.color
                }
                if (d.series != undefined) {
                    if (series.indexOf(d.series) < 0) {
                        series.push(d.series);
                    }
                    console.log(d.series, series, series.indexOf(d.series));
                    return cfg.seriesColor[series.indexOf(d.series)];
                }
                return cfg.color
            }
            )
            .style("stroke-width", function (d) { if (d.lineWidth != undefined) { return d.lineWidth } return cfg.lineWidth })
            .style("fill", function (d) { if (d.background != undefined) { return d.background } return cfg.background })
            .attr("cy", function (d) {
                if (cfg.horizontalLayout) {
                    return Math.floor(cfg.height / 2)
                }
                var datum = (cfg.dateDimension) ? new Date(d.date).getTime() : d.value;
                return Math.floor(step * (datum - minValue) + margin)
            })
            .attr("cx", function (d) {
                if (cfg.horizontalLayout) {
                    var datum = (cfg.dateDimension) ? new Date(d.date).getTime() : d.value;
                    var x = Math.floor(step * (datum - minValue) + margin);
                    return x;
                }
                return Math.floor(cfg.width / 2)
            })
            .on("mouseover", function (d) {
                if (cfg.dateDimension) {
                    var format = d3.time.format(cfg.dateFormat);
                    var datetime = format(new Date(d.date));
                    var dateValue = (datetime != "") ? (d.name + " <small>(" + datetime + ")</small>") : d.name;
                } else {
                    var format = function (d) { return d }; // TODO
                    var datetime = d.value;
                    var dateValue = d.name + " <small>(" + (d.desc || '') + ")</small>";
                }
                d3.select(this)
                    .style("fill", function (d) {

                        if (d.color != undefined) {
                            return d.color
                        }

                        if (d.background != undefined) {
                            return d.background
                        }

                        return cfg.color;
                    }).transition()
                    .duration(100).attr("r", function (d) { if (d.radius != undefined) { return Math.floor(d.radius * 1.5) } return Math.floor(cfg.radius * 1.5) });
                tip.html("");
                if (d.img != undefined) {
                    tip.append("img").style("float", "left").style("margin-right", "4px").attr("src", d.img).attr("width", "64px");
                }
                tip.append("div").style("float", "left").html(dateValue);
                tip.transition()
                    .duration(100)
                    .style("opacity", .9);

            })
            .on("mouseout", function () {
                d3.select(this)
                    .style("fill", function (d) { if (d.background != undefined) { return d.background } return cfg.background }).transition()
                    .duration(100).attr("r", function (d) { if (d.radius != undefined) { return d.radius } return cfg.radius });
                tip.transition()
                    .duration(100)
                    .style("opacity", 0)
            });




        for (var i = 0; i < svg.selectAll("circle")._groups[0].length; i++) {

            var s = svg.selectAll("circle")._groups[0][i];
            var x = s.getAttribute('cx');
            var y = s.getAttribute('cy');

            var name = events[i].name;
            var desc = events[i].desc;
            var p = events[i].p;




            text = svg.append("text").style("font-size", "70%")
                .attr("x", function (d) {

                    return x - 15;
                }
                )
                .attr("y", function (d) {

                    if (p == "t") {
                        return y - 65;

                    }
                    return (150);


                }
                )
                .attr("dy", "0")
                .style("border", "1px green solid");

            text.append("tspan")
                .attr("dy", "1")
                .attr("x", function (d) {

                    return x - 15;
                })
                .text(name);

            if (desc) {
                text.append("tspan")
                    .attr("x", function (d) {

                        return x - 15;
                    })
                    .attr("dy", "15")
                    .text(desc);

            }




            bbox = text._groups[0][0].getBBox();
            ctm = text._groups[0][0].getCTM();

            var padigrec = 10;

            rect = svg.insert('rect', 'text')
                .attr('x', bbox.x - 5)
                .attr('y', bbox.y - 5)
                .attr('width', bbox.width + padigrec)
                .attr('height', bbox.height + padigrec)
                .attr('stroke-width', 1)
                .attr('stroke', "#3580b8")
                .attr('fill', "#fff")


            if (p == "t") {
                svg.append("line")
                    .attr("class", "timeline-line")
                    .attr("x1", function (d) {

                        return x;
                    })
                    .attr("x2", function (d) {

                        return x;
                    })
                    .attr("y1", function (d) {

                        return y;
                    })
                    .attr("y2", function (d) {
                        return y - (bbox.y + bbox.height - padigrec -3);
                    })
                    .style("stroke", function (d) {
                        return "#3580b8";
                    })
                    .style("stroke-width", cfg.lineWidth)
            }



            //svg.append("foreignObject").attr("class", "node").style("font-size", "70%").attr("width", "100").attr("height", "100")
            //    .attr("x", function (d) {

            //        return x - 15;
            //    }
            //    )
            //    .attr("y", function (d) {

            //        if (p == "t") {
            //            return y - 35;

            //        }
            //        return (150);


            //    }
            //    ).append("div").text(name).style("border", "1px green solid");

        }





        //Adding start and end labels
        if (cfg.showLabels != false) {
            if (cfg.dateDimension) {
                var format = d3.time.format(cfg.labelFormat);
                var startString = format(new Date(minValue));
                var endString = format(new Date(maxValue));
            } else {
                var format = function (d) { return d }; //Should I do something else?
                var startString = minValue;
                var endString = maxValue;
            }
            svg.append("text")
                .text(startString).style("font-size", "70%")
                .attr("x", function (d) { if (cfg.horizontalLayout) { return d3.max([0, (margin - this.getBBox().width / 2)]) } return Math.floor(this.getBBox().width / 2) })
                .attr("y", function (d) { if (cfg.horizontalLayout) { return Math.floor(cfg.height / 2 + (margin + this.getBBox().height)) } return margin + this.getBBox().height / 2 });

            svg.append("text")
                .text(endString).style("font-size", "70%")
                .attr("x", function (d) { if (cfg.horizontalLayout) { return cfg.width - d3.max([this.getBBox().width, (margin + this.getBBox().width / 2)]) } return Math.floor(this.getBBox().width / 2) })
                .attr("y", function (d) { if (cfg.horizontalLayout) { return Math.floor(cfg.height / 2 + (margin + this.getBBox().height)) } return cfg.height - margin + this.getBBox().height / 2 })
        }


        svg.on("mousemove", function () {
            tipPixels = parseInt(tip.style("height").replace("px", ""));
            return tip.style("top", (d3.event.pageY - tipPixels - margin) + "px").style("left", (d3.event.pageX + 20) + "px");
        })
            .on("mouseout", function () { return tip.style("opacity", 0).style("top", "0px").style("left", "0px"); });
    }
}

