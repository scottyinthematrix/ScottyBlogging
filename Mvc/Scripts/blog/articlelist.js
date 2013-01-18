


Ext.define('Article', {
	extend: 'Ext.data.Model',
	fields: [
		{ name: 'ID', type: 'string' },
		{ name: 'Title', type: 'string' },
		{ name: 'CreateDate', type: 'string' },
		{ name: 'ModifyDate', type: 'string' },
		{ name: 'Body', type: 'string' }
	]
});

var articleStore = Ext.create('Ext.data.Store', {
	storeId: 'articleStore',
	model: 'Article',
	proxy: {
		type: 'ajax',
		url: getFullUrl('Article/List'),
		reader: {
			type: 'json',
			root: 'Data',
			idProperty: 'ID',
			totalProperty: 'total'
		}
	},
	autoLoad: true
});

function renderDate(value) {
	var date = Ext.Date.parse(value, "MS");
	return Ext.Date.format(date, 'Y-m-d H:i:s');
	
	//var re = /\/Date\((\d+)\)\//;
	//var ms = value.replace(re, "$1");
	//var date = new Date(parseInt(ms));
	//return Ext.Date.format(date, 'Y-m-d H:i:s');
	
}

function renderLink(value) {
    var url = getFullUrl(Ext.String.format('/Article/Edit/{0}', value));
    return Ext.String.format('<a href="{0}">Edit</a>', url);
}

$(document).ready(function() {
	Ext.create('Ext.grid.Panel', {
		title: 'articles',
		store: articleStore,
		columns: [
			{ text: 'ID', dataIndex: 'ID' },
			{ text: 'Title', dataIndex: 'Title' },
			{ text: 'Created', dataIndex: 'CreateDate', renderer: renderDate },
			{ text: 'Last Modified', dataIndex: 'ModifyDate', renderer: renderDate },
			{ text: 'Content', dataIndex: 'Body' },
		    { text: '', dataIndex: 'ID', renderer: renderLink }
		],
		renderTo: Ext.get('grid')
	});
}); 