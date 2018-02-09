const path = require('path');

module.exports = {
    devtool: "sourcemap",
    entry: "./index.ts",
    context: path.resolve(__dirname, "src"),
    output: {
        path: path.resolve(__dirname, './dist'),
        filename: "simple-server-client.js",
        library : 'JwtSimpleServer',
        libraryTarget: 'window'
    },
    resolve: {
        extensions: ['.js', '.ts']
    },
    module: {
        loaders: [
            {
                loader: 'ts-loader',
                test: /\.ts$/,  
                exclude:  /node_modules/                
            }
        ]
    }
}