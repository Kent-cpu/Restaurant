import React, {useContext, useState} from 'react';
import {Button, CloseButton, Modal} from "react-bootstrap";
import {deleteTable} from "../../http/tableApi";
import s from "./Table.module.css";
import {AuthContext} from "../../contexts";
import {ROLES} from "../../utils/roles";

const Table = ({id, name, capacity, setTables, reserveTable}) => {
    const {user} = useContext(AuthContext);
    const [showModal, setShowModal] = useState(false);


    const delTable = async () => {
        try {
            await deleteTable(id);
            setTables(tables =>  tables.filter(table => table.id != id));
        }catch (e) {
            console.log(e);
        }
    }

    const reserve = async () => {
        try {
            await reserveTable(id);
            setShowModal(false);
            setTables(tables => tables.filter(table => table.id != id));
        }catch (e) {
            console.log(e);
        }
    }

    const handleCloseModal = (e) => {
        e.stopPropagation();
        setShowModal(false);
    }

    const handleOpenModal = (e) => {
        if(user.role === ROLES.USER) {
            setShowModal(true);
        }
    };


    return (
        <div onClick={handleOpenModal} className={s["table"]}>
                <p className="mb-0">{name}</p>
                <p className="mb-0">Вместимость: {capacity}</p>
                {
                    user.role === ROLES.ADMIN &&
                        <CloseButton
                            className={s["close-btn"]}
                            onClick={delTable}
                        />
                }

            <Modal show={showModal}>
                <Modal.Header>
                    <Modal.Title>Подтверждение бронирования</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>Вы уверены, что хотите забронировать этот столик?</p>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseModal}>
                        Отмена
                    </Button>
                    <Button variant="primary" onClick={reserve}>
                        Подтвердить
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default Table;