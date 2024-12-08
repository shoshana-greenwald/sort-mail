import * as React from 'react';
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Fade from '@mui/material/Fade';
import EditIcon from '@mui/icons-material/Edit';
import { Box } from '@mui/material';
import axios from "axios";
import swal from 'sweetalert';
import CircularProgress from '@mui/material/CircularProgress';
import LoadingButton from '@mui/lab/LoadingButton';
import { connect } from 'react-redux';


type subjectsType = {
    subjectId: number;
    subjectName: string;
    parentID: number;
    userId: number;
    color: string;
}
type CustomizedListProps = {
    renderfunc: () => void;
    thisSubject: number;
    thisColor:string;
}

// קומפוננטה האחראית על שינוי צבע לנושא קיים
export default function ChangeColor(props:CustomizedListProps) {

    const [color, setColor] = React.useState<string>(props.thisColor);

    // פונקציה ששולחת צבע חדש לשרת, בעת שינוי צבע בפיקר
    async function changeColor(c:string){
        let data = {newColor:c,subjectId:props.thisSubject};
        const response = await axios.post("https://localhost:44391/api/Subject/ChangeSubjectColor", data);
        let message = response.data;

        if (message === "Success") {  
            props.renderfunc();
            setColor(c);
        }

    }

    return (
        <input style={{cursor:"pointer"}} type = "color" value = {color} onChange={e => changeColor(e.target.value)} ></input>
    );
}

