
var trees = [{
     "id": 1,
     "label": "岩石学特征",
     "parent_id": null,
     "url": null,
     "depth": 0,
     "child_num": 3,
     "paramvalue": "",
     "children": [{
         "id": 2,
         "label": "岩石颜色",
         "parent_id": 1,
         "url": null,
         "depth": 1,
         "child_num": 5,
         "paramvalue": "黑色",
         "sampledata": "典型数据是黑色",
         "remark": "补充数据是我",
         "expanded":true,
         "children": [{
             "id": 3,
             "label": "Menus",
             "parent_id": 2,
             "url": "/menus",
             "depth": 2,
             "child_num": 0,
             "paramvalue": "menu manager",
         }, {
             "id": 4,
             "label": "Roles",
             "parent_id": 2,
             "url": "/roles",
             "depth": 2,
             "child_num": 0,
             "paramvalue": "Role Manager",
         }, {
             "id": 5,
             "label": "Users",
             "parent_id": 2,
             "url": "/users",
             "depth": 2,
             "child_num": 0,
             "paramvalue": "User Manager",
         }]
     }]
 }, {
     "id": 6,
     "label": "Customs",
     "parent_id": null,
     "url": null,
     "depth": 0,
     "child_num": 2,
     "paramvalue": "Custom Manager",
     "children": [{
         "id": 7,
         "label": "CustomList",
         "parent_id": 6,
         "url": "/customs",
         "depth": 1,
         "child_num": 0,
         "paramvalue": "CustomList",
     }]
 }, {
     "id": 8,
     "label": "Templates",
     "parent_id": null,
     "url": null,
     "depth": 0,
     "child_num": 1,
     "paramvalue": "Template Manager",
     "children": [{
         "id": 9,
         "label": "TemplateList",
         "parent_id": 8,
         "url": "/doc_templates",
         "depth": 1,
         "child_num": 0,
         "paramvalue": "Template Manager",
     }]
 }, {
     "id": 10,
     "label": "Bussiness",
     "parent_id": null,
     "url": null,
     "depth": 0,
     "child_num": 2,
     "paramvalue": "Bussiness Manager",
     "expanded":true,
     "children": [{
         "id": 11,
         "label": "BussinessList",
         "parent_id": 10,
         "url": null,
         "depth": 1,
         "child_num": 2,
         "paramvalue": "BussinessList",
         "children": [{
             "id": 12,
             "label": "Currencies",
             "parent_id": 11,
             "url": "/currencies",
             "depth": 2,
             "child_num": 0,
             "paramvalue": "Currencies",
         }, {
             "id": 13,
             "label": "Dealtypes",
             "parent_id": 11,
             "url": "/dealtypes",
             "depth": 2,
             "child_num": 0,
             "paramvalue": "Dealtypes",
         }]
     }, {
         "id": 14,
         "label": "Products",
         "parent_id": 10,
         "url": null,
         "depth": 1,
         "child_num": 2,
         "paramvalue": "Products",
         "children": [{
             "id": 15,
             "label": "ProductTypes",
             "parent_id": 14,
             "url": "/productTypes",
             "depth": 2,
             "child_num": 0,
             "paramvalue": "ProductTypes",
         }, {
             "id": 16,
             "label": "ProductList",
             "parent_id": 14,
             "url": "/products",
             "depth": 2,
             "child_num": 0,
             "paramvalue": "ProductList",
         }]
     }]
 }]