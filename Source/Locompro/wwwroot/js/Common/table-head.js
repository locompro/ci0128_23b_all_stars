class TableHead {
    constructor(pageConfiguration, table) {
        this.table = table;
        this.pageConfiguration = pageConfiguration;
        this.generateTableHead();
    }
    
    generateTableHead() {
        this.table.innerHTML = "";
        const tableHead = document.createElement("thead");
        this.table.appendChild(tableHead);
        
        const tr = document.createElement("tr");
        tableHead.appendChild(tr);

        for (let headerField of this.pageConfiguration.headerFields) {
            headerField.createHeader(tr);
        }
    }
    
    updateHead(currentOrder) {
        if (this.pageConfiguration.headerOrderingElements === null ||
            this.pageConfiguration.headerOrderingElements.length === 0) {
            return;
        }

        for (const header of this.pageConfiguration.headerOrderingElements) {
            header.remove("sort-asc");
            header.remove("sort-desc");
            
            header.add("no-underline-link");
            if (header.attribute === currentOrder.attribute) {
                if (currentOrder.isDescending) {
                    header.add("sort-desc");
                } else {
                    header.add("sort-asc");
                }
            }
        }
    }
}

class HeaderField {
    constructor(name, isOrderable, attribute = "") {
        this.name = name;
        this.id = name + (isOrderable ? "SortButton" : "Header");
        this.isOrderable = isOrderable;
        this.attribute = attribute;
        this.styles = [];
    }

    createHeader(tableHead) {
        const header = document.createElement("th");
        header.scope = "col";
        
        this.applyStyles(header);

        if (this.isOrderable) {
            const content = document.createElement("a");
            content.id = this.id;
            content.innerHTML = this.name;

            content.classList.add("no-underline-link");
            content.onclick = () => {
                getPage().setOrder(this.attribute);
            }

            header.appendChild(content);
        } else {
            header.id = this.id;
            header.innerHTML = this.name;
        }

        tableHead.appendChild(header);
    }
    
    addStyle(styleProperty, styleValue) {
        this.styles.push({property: styleProperty, value: styleValue});
    }
    
    applyStyles(element) {
        for (const style of this.styles) {
            element.style[style.property] = style.value;
        }
    }
}

export {TableHead, HeaderField};