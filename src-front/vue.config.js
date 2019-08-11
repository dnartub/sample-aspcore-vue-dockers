module.exports = {
	css: {
		loaderOptions: {
			less: {
				modifyVars: {
				},
				javascriptEnabled: true
			},
			sass: {
				// @/ is an alias to src/
				// so this assumes you have a file named `src/variables.scss`
				//data: `@import "~@/styles/_base-functions.scss";`
			}
		}
    },
    chainWebpack: config => {
    },
    configureWebpack: {
        //performance: {
        //    hints: false
        //},
        //optimization: {
        //    splitChunks: {
        //        minSize: 10000,
        //        maxSize: 250000
        //    }
        //}
    }
};
