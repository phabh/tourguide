var express = require('express');
var app = express();

module.exports = {
    init: function () {
        app.listen(3000, function () {
            console.log('Example app listening on port 3000!');
        });
    },
    getApp: function () {
        return app;
    }
};