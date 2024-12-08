import React from 'react';
import { connect } from 'react-redux';
import { GridColDef } from '@mui/x-data-grid';
import { DataGrid } from '@mui/x-data-grid';
//import { DataGrid, GridColDef, GridValueGetterParams, GridCellParams } from '@mui/x-data-grid';
import clsx from 'clsx';
import { Box } from '@mui/material';
import axios from "axios";
import { useState, useEffect } from 'react';
import Autocomplete from '@mui/material/Autocomplete';
import ChangeClassification from "./ChangeClassification";
import ChangeColor from "./ChangeColor";
import RemoveSubject from "./RemoveSubject";
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import AddIcon from '@mui/icons-material/Add';
import IconButton from '@mui/material/IconButton';
import AddNewSubjet from './AddNewSubjet';


type subjectsType = {
    subjectId: number;
    subjectName: string;
    parentID: number;
    userId: number;
    color: string;
}


type stateList={
    cangeFilter: (newFilter: string) => void;
    renderfunc: ()=>void;
    subjectsFromServer:any; 
}

export default function SubTable(props: stateList) {

    const columns: GridColDef[] = [
        {
            field: 'subjectLebel',
            headerName: 'תוית נושא',
            width: 230,
            cellClassName: "subjectCell",
            renderCell: (params) => <div onClick={() => props.cangeFilter(params.row.subjectName)} className="lebelSubject" style={{ cursor: "pointer", backgroundColor: params.row.color }}> {params.row.subjectName}</div>,

        },
        { field: 'subjectName', headerName: 'נושא', width: 200 },
        { field: 'parentName', headerName: 'נושא אב', width: 480 },
        {
            field: 'chamgeColor',
            headerName: 'שינוי צבע',
            width: 120,
            sortable: false,

            renderCell: (params) => {
                return <ChangeColor thisSubject={params.row.subjectId} thisColor={params.row.color} renderfunc={props.renderfunc}  ></ChangeColor>
            }
        },
        {
            field: 'remove',
            headerName: 'מחיקת הנושא',
            description: 'בעת מחיקה נמחקים כל הנתונים שנאגרו על הנושא',
            width: 120,
            sortable: false,

            renderCell: (params) => {
                return <RemoveSubject thisSubjectId={params.row.subjectId} thisSubjectName={params.row.subjectName} renderfunc={props.renderfunc}  ></RemoveSubject>
            }
        },
    ];

    let rows: {
        subjectId: number;
        subjectName: string;
        parentName: (string | undefined);
        userId: number;
        color: string;
    }[] = [];
    let tempRows = rows;

    let setRows: (arg0: any) => void;
    [rows, setRows] = useState([]);


    useEffect(() => {
        const getSubjects = async () => {
            const subjectsFromServer: subjectsType[] = Object.values(props.subjectsFromServer);
            subjectsFromServer.forEach((sub) => {
                let parentSub: (string | undefined) = subjectsFromServer.find((item) =>
                    item.subjectId == sub.parentID
                )?.subjectName;
                tempRows.push({ userId: sub.userId, color: sub.color, subjectName: sub.subjectName, subjectId: sub.subjectId, parentName: parentSub })
            })
            setRows({ ...tempRows });
        };
        getSubjects();

    }, [props.subjectsFromServer]);


    return (<>
        {rows &&
            <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={1} sx={{
                height: "100%",}}>
                <Box gridColumn="span 12" >
                    <Box
                        sx={{
                            height: "57vh",
                            width: 1,
                            direction: 'rtl',
                            '& ::selection': {
                                background: "#00000000"
                            },
                            '& .css-vmke14 .css-34fkgz .css-108b71r .css-car4ek .css-yywbvm .css-rvf4dy .css-2hlayk .css-1w3l6uw .css-w78mzj .css-uwn239': {
                                height: "77% !important"
                            },
                            '& .css-17jjc08-MuiDataGrid-footerContainer': {
                                direction: "ltr",
                                display: "none"
                            },
                            '& .css-3ihp42-MuiDataGrid-root': {
                                width: '75vw',
                                backgroundColor: "whitesmoke",
                                borderRadius: "15px",
                                margin: "0 3.2vw 0 1vw",
                                height: "71.5%"

                            },
                            '& .super-app-theme--cell': {
                                backgroundColor: 'rgba(224, 183, 60, 0.55)',
                                color: '#1a3e72',
                                fontWeight: '600',
                            },
                            '& .super-app.negative': {
                                color: 'rgba(157, 255, 118, 0.49)',
                                fontWeight: '600',
                            },
                            '& .super-app.positive': {
                                color: '#d47483',
                                fontWeight: '600',
                                // backgroundColor:'salmon',
                                // padding:'3px 10px',
                                // borderRadius:'17px'
                            },
                            '& .sub': {
                                backgroundColor: 'salmon',
                                padding: '3px 10px',
                                borderRadius: '17px'
                            },
                            '& .lebelSubject': {
                                backgroundColor: "pink",
                                borderRadius: "15px",
                                padding: "3px 14px"
                            },
                            '& .css-3ihp42-MuiDataGrid-root .MuiDataGrid-columnHeader:focus-within, .css-3ihp42-MuiDataGrid-root .MuiDataGrid-cell:focus-within': {
                                outline: "none"
                            },
                            '& .subjectCell': {
                                justifyContent: "center !important",
                            }

                        }}
                    >
                        <div dir='rtl' style={{ height: 600, width: '70%' }}>
                            <DataGrid getRowId={row => row.subjectId}
                                rows={Object.values(rows)}
                                columns={columns}
                                pageSize={8}
                                rowsPerPageOptions={[5]}
                            />
                        </div>
                    </Box>
                </Box>
                <Box gridColumn="span 12" >
                    <AddNewSubjet renderfunc={props.renderfunc} subjects={Object.values(rows)}></AddNewSubjet>
                </Box>
            </Box>


        }

    </>
    );
}




