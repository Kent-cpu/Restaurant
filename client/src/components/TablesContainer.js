import React from 'react';
import Table from "./Table/Table";

const TablesContainer = ({tables, setTables, reserveTable}) => {
    return (
        <div style={
            {
                display: "grid",
                gridTemplateColumns: "repeat(auto-fill, minmax(160px, 1fr))",
                columnGap: "10px",
                rowGap: "12px",
            }
        }>
            {
                tables.map(table => {
                    return <Table
                        key={table.id}
                        id = {table.id}
                        name={table.name}
                        capacity={table.capacity}
                        setTables={setTables}
                        reserveTable={reserveTable}
                    />
                })
            }
        </div>
    );
};

export default TablesContainer;