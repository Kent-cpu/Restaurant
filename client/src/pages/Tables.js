import React, {useEffect, useState} from 'react';
import {Button, Container, Form, Modal} from "react-bootstrap";
import Header from "../components/Header";
import {createTable, getTables} from "../http/tableApi";
import TablesContainer from "../components/TablesContainer";

const Tables = () => {
    const [tables, setTables] = useState([]);
    const [tableName, setTableName] = useState('');
    const [capacity, setCapacity] = useState('');
    const [showModal, setShowModal] = useState(false);


    useEffect(() => {
        getTables()
            .then(tables => setTables(tables))
            .catch(e => console.log(e));
        ;
    }, []);

    const saveTable = async () => {
        try {
            if(tableName === '' || capacity === '') {
                alert("Введите название столика и вместимость");
                return;
            }
            const response = await createTable(tableName, capacity);
            setTables(tables =>
                [...tables, {id: response.id, name: response.name, capacity: response.capacity}]
            );
            setShowModal(false);
        } catch (e) {
            console.log(e);
        }
    }

    const handleOpenModal = () => {
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setShowModal(false);
    };

    return (
        <div>
            <Header/>
            <Container>
                <Button onClick={handleOpenModal} className="mb-4">Добавить столик</Button>
                <TablesContainer
                    tables={tables}
                    setTables={setTables}
                />
                <Modal show={showModal} onHide={handleCloseModal}>
                    <Modal.Header closeButton>
                        <Modal.Title>Добавить новый столик</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form>
                            <Form.Group className="mb-3" controlId="tableName">
                                <Form.Label>Название столика</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={tableName}
                                    onChange={(e) => setTableName(e.target.value)}
                                />
                            </Form.Group>
                            <Form.Group className="mb-2" controlId="capacity">
                                <Form.Label>Вместимость</Form.Label>
                                <Form.Control
                                    type="number"
                                    value={capacity}
                                    onChange={(e) => setCapacity(e.target.value)}
                                />
                            </Form.Group>
                        </Form>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={saveTable}>Добавить</Button>
                        <Button onClick={handleCloseModal}>Закрыть</Button>
                    </Modal.Footer>
                </Modal>
            </Container>
        </div>
    );
};

export default Tables;