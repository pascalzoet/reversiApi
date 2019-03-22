this["reversi_templates"] = this["reversi_templates"] || {};
this["reversi_templates"]["src"] = this["reversi_templates"]["src"] || {};
this["reversi_templates"]["src"]["templates"] = this["reversi_templates"]["src"]["templates"] || {};
this["reversi_templates"]["src"]["templates"]["gameplay"] = this["reversi_templates"]["src"]["templates"]["gameplay"] || {};
this["reversi_templates"]["src"]["templates"]["gameplay"]["gameplay"] = Handlebars.template({"compiler":[7,">= 4.0.0"],"main":function(container,depth0,helpers,partials,data) {
    return "";
},"useData":true});
this["reversi_templates"]["src"]["templates"]["gameplay"]["onset"] = Handlebars.template({"compiler":[7,">= 4.0.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=helpers.helperMissing, alias3="function", alias4=container.escapeExpression;

  return "<div id=\"advertisement\">\r\n		<div class=\"header\">Temperatuurmeter</div>\r\n		<div class=\"place-title\">"
    + alias4(((helper = (helper = helpers.place || (depth0 != null ? depth0.place : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"place","hash":{},"data":data}) : helper)))
    + "</div>\r\n		<div class=\"temp\">"
    + alias4(((helper = (helper = helpers.temp || (depth0 != null ? depth0.temp : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"temp","hash":{},"data":data}) : helper)))
    + "</div>\r\n	</div>";
},"useData":true});
this["reversi_templates"]["src"]["templates"]["gameplay"]["whois"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    return "    <div class=\"stone-block-black\">\r\n        <span class=\"stone black\"></span>\r\n    </div>\r\n";
},"3":function(container,depth0,helpers,partials,data) {
    return "        <div class=\"stone-block-white\">\r\n        <span class=\"stone white\"></span>\r\n    </div>\r\n";
},"compiler":[7,">= 4.0.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, helper, alias1=depth0 != null ? depth0 : (container.nullContext || {});

  return "<div id=\"whois\">\r\n"
    + ((stack1 = helpers["if"].call(alias1,(depth0 != null ? depth0.black : depth0),{"name":"if","hash":{},"fn":container.program(1, data, 0),"inverse":container.program(3, data, 0),"data":data})) != null ? stack1 : "")
    + "    <div class=\"stone-type\">\r\n        <span>Aan zet</span>\r\n    </div>\r\n    <div class=\"stone-type you-are\">\r\n        <span>"
    + container.escapeExpression(((helper = (helper = helpers.you || (depth0 != null ? depth0.you : depth0)) != null ? helper : helpers.helperMissing),(typeof helper === "function" ? helper.call(alias1,{"name":"you","hash":{},"data":data}) : helper)))
    + "</span>\r\n    </div>\r\n    <div class=\"skip-turn\">\r\n        <button onclick=\"Spa.Reversi.skip()\" id=\"skipturn\">Sla beurt over</button>\r\n    </div>\r\n</div>";
},"useData":true});