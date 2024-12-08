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



type subjectsType = {
    subjectId: number;
    subjectName: string;
    parentID: number;
    userId: number;
    color: string;
}

type stateList={
    cangeFilter: (newFilter: string) => void;
    filter:string;
    renderfunc: ()=>void;
    emailsFromServer:any; 
}

//קומפוננטה האחראית על תצוגת הטבלה של המיילים
function DataTable(props: stateList) {

    const columns: GridColDef[] = [
        {
            field: 'email',
            headerName: 'מייל',
            sortable: false,
            width: 200,
            renderCell: (params) => <div style={{ direction: "ltr" }}> {params.value}</div>,
        },
        { 
            field: 'name', 
            headerName: 'שם השולח', 
            width: 110
        },
        {
            field: 'date',
            headerName: 'תאריך',
            width: 110,
        },
        {
            field: 'short',
            headerName: 'תמצות',
            width: 490,
            sortable: false,

        },
        {
            field: 'subject',
            headerName: 'נושא',
            width: 130,
            cellClassName: "subjectCell",
            renderCell: (params) => {
                if(params.value == "") 
                    return <div></div>;
                else
                    return <div onClick={() => props.cangeFilter(params.value)} className="lebelSubject" style={{ backgroundColor: params.row.color, cursor: "pointer" }}> {params.value}</div>
            }
        },
        {
            field: 'edit',
            headerName: 'סיווג חדש',
            description: 'ניתן לשנות את קטגורית המייל עד לשבוע אחרי הקבלה',
            width: 100,
            sortable: false,
            renderCell: (params) => {
                const subjects: subjectsType[] = params.row.userSubjets;
                const num_integer: number = parseInt(params.id.toString());
                return <ChangeClassification date = {params.row.date} thisSubject={params.row.subject} mailId={num_integer} renderfunc={props.renderfunc} subjetsList={subjects} ></ChangeClassification>
            }
        },
    ];


    let rows: {
        color: string;
        toUserId: number;
        id: number;
        email: string;
        name: string;
        date: string;
        short: string;
        subject: string;
        userSubjets: subjectsType[];
    }[] = [];
    let tempRows = rows;
    const { emailsFromServer } = props;

    let setRows: (arg0: any) => void;
    [rows, setRows] = useState([]);

    const [filter, setFilter] = useState<string>(props.filter);


    useEffect(() => {
        const getEmails = async () => {
            tempRows = Object.values(props.emailsFromServer);
            if (props.filter != "$all") {
                tempRows = tempRows.filter(x => x.subject == props.filter);
            }
            setRows({ ...tempRows });

        };
        getEmails();

    }, [props.filter, props.emailsFromServer]);


    return (<>
        {rows && <Box
            sx={{
                height: 300,
                width: 1,
                direction: 'rtl',
                '& ::selection': {
                    background: "#00000000"
                },
                '& .css-17jjc08-MuiDataGrid-footerContainer': {
                    direction: "ltr",
                },
                '& .css-3ihp42-MuiDataGrid-root': {
                    width: '75vw',
                    direction: "rtl",
                    backgroundColor: "whitesmoke",
                    borderRadius: "15px",
                    margin: "0 3.2vw 0 1vw",
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
            <div dir='rtl' style={{ height: 527, width: '70%' }}>
                <DataGrid
                    rows={Object.values(rows)}
                    columns={columns}
                    pageSize={8}
                    rowsPerPageOptions={[5]}
                />
            </div>
        </Box>
        }
    </>
    );
}




// מוסיף את הנתונים לתוך הפרופס של הקומפוננטה
const mapStateToProps = (globalState: any) => {
    return {
        email: globalState.email,
        password: globalState.password
    };
};

// הקומפוננטה הזו אמורה לקבל ערכים מהסטייט
// connect זה הפרמטר הראשון שמקבלת הפונקציה

// mapStateToProps - מקבלת ערכים מהסטייט הכללי
// mapDispatchToProps - משגרת ערכים לסטייט הכללי - הפונקציות שמשנות - actions
export default connect(mapStateToProps)(DataTable);