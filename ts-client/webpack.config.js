const path = require('path');

module.exports = {
    entry: "./src/index.ts",
    output: {
        path: path.resolve(__dirname, '../../dist'),
        filename: "simple-server-client.js"
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