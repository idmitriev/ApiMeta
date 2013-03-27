(function () {
    var root = this;
    var _require = function(moduleName) {
        var module = root[moduleName];
        if (!module && (typeof require !== 'undefined'))
            module = require('underscore');
        return module;
    };

    var _ = _require('_');
    var $ = _require('$');

    var apiclient;
    if (typeof exports !== 'undefined') 
        apiclient = exports;
    else 
        apiclient = root.apiclient = {};
    
    apiclient.settings = apiclient.settings || {};
    apiclient.network = apiclient.network || {};

    apiclient.network.request = function(options) {
        options = options || {};
        options.traditional = true;
        return $.ajax(options);
    };

    apiclient.buildUrl = function (uriTemplate, parameters, values) {
        var urlParameters = _.chain(parameters).filter(function(parameter) {
            return parameter.source == "uri";
        });

        if (urlParameters.value().length == 1 && !_.isObject(values))
            return uriTemplate.replace(/\{.+\}/g, values);

        urlParameters.map(function(parameter) {
            return parameter.name;
        }).each(function(parameterName) {
            uriTemplate = uriTemplate.replace('{' + parameterName + '}', values[parameterName]);
        });
        return uriTemplate;
    };

    apiclient.buildHeaders = function(request, credentials) {
        if (request.authenticationType == "BasicAuth")
            return { 'Authorization': "Basic " + btoa(credentials.username + ':' + credentials.password) };
    };

    apiclient.buildData = function(parameters, values) {
        var data = {};
        _.chain(parameters).filter(function(parameter) {
            return parameter.source == "body";
        }).map(function(parameter) {
            return parameter.name;
        }).each(function(parameterName) {
            data[parameterName] = values[parameterName];
        });

        return _.keys(data).length == 1 ? values : data;
    };

    apiclient.build = function(options) {
        var apiRootUrl = (_.isString(options) ? options : apiclient.settings.url);
        if (apiRootUrl && apiRootUrl[apiRootUrl.length - 1] != '/')
            apiRootUrl += '/';
        var metadataUrl = apiRootUrl + 'resourcemetadata';
        
        var api = {},
            getMetadata = _.isArray(options) ?
                (new $.Deferred()).when(function() { return options; }) :
                apiclient.network.request({ url: metadataUrl, type: 'get', dataType: 'json' });

        return getMetadata.then(function(metadata) {
            _.each(metadata, function(resource) {
                api[resource.name] = {};
                _.each(resource.requests, function(request) {
                    api[resource.name][request.name] = function(params) {
                        params = params || {};
                        
                        var url = (apiclient.settings.url || apiRootUrl);
                        if (url && url[url.length - 1] != '/')
                            url += '/';
                        url += apiclient.buildUrl(request.uri, request.parameters, params);

                        if (request.requiresHttps)
                            url = url.replace('http:', 'https:');

                        var headers = apiclient.buildHeaders(request, params.credentials || api.credentials || { username:'', password: '' });
                        var data = apiclient.buildData(request.parameters, params);

                        return apiclient.network.request({
                            type: request.method,
                            data: data,
                            url: url,
                            dataType: 'json',
                            headers: headers
                        }); 
                    };
                });
            });

            return api;
        });
    };
})(this);