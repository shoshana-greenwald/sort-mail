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
import Tooltip from '@mui/material/Tooltip';



type subjectsType = {
    subjectId: number;
    subjectName: string;
    parentID: number;
    userId: number;
    color: string;
}
type CustomizedListProps = {
    mailId: number;
    date: string;
    subjetsList: subjectsType[];
    renderfunc: () => void;
    thisSubject: string;
}

//קומפוננטה האחראית על שינוי קטגורית המייל -- האייקון השמאלי
export default function ChangeClassification(props: CustomizedListProps) {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };
    //פונקציה המחזירה הפרש בימים בין שתי תאריכים
    function getDifferenceInDays(date1: any, date2: any) {
        const diffInMs = Math.abs(date2 - date1);
        return Math.floor( diffInMs / (1000 * 60 * 60 * 24));
    }

    const current = new Date();
    const lst = props.date.split('-');
    const dateReceived = new Date(`${lst[1]}/${lst[2]}/${lst[0]}`);

    const mailId = props.mailId;
    const [subjetsList, setSubjetsList] = React.useState<subjectsType[]>([]);
    const [loading, setLoading] = React.useState<boolean>(false);
    React.useEffect(() => {
        setSubjetsList(props.subjetsList.filter((sub) => sub.subjectName !== props.thisSubject));
    }, [props.thisSubject, props.subjetsList])


    //אחראית על שליחה לשרת בעת לחיצה על הנושא החדש
    async function ChangeSubject(emailId: number, newSubject: string) {
        setAnchorEl(null);
        setLoading(true);

        let data = {
            emailId,
            newSubject
        }
        const response = await axios.post("https://localhost:44391/api/Subject/ChangeEmailSubject", data);
        let message = response.data;

        if (message === "Success") {
            props.renderfunc();
        }
        setLoading(false);

    }


    return (
        <div>
            <Box
                sx={{
                    '& button': {
                        borderRadius: "50px !important",
                        height: "35px !important",
                        minWidth: "10px !important",
                        width: "35px !important",
                        marginRight: "15px",
                    }
                }}>
                <LoadingButton
                    id="fade-button"
                    aria-controls={open ? 'fade-menu' : undefined}
                    aria-haspopup="true"
                    aria-expanded={open ? 'true' : undefined}
                    onClick={handleClick}
                    loading={loading}
                    disabled={getDifferenceInDays(current, dateReceived) > 7}
                >
                    <Tooltip title={`נשארו לך ${7 - getDifferenceInDays(current, dateReceived)} ימים`}>
                        <EditIcon />
                    </Tooltip>

                </LoadingButton>
            </Box>
            <Menu
                id="fade-menu"
                MenuListProps={{
                    'aria-labelledby': 'fade-button',
                }}
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                TransitionComponent={Fade}
            >

                {Object.values(subjetsList).map((sub: subjectsType) =>
                    <MenuItem
                        onClick={(e) => ChangeSubject(mailId, sub.subjectName)}
                        style={{ textAlign: "center", flexDirection: "row-reverse" }}
                        key={sub.subjectId}>
                        {sub.subjectName}
                    </MenuItem>
                )}

            </Menu>
        </div>
    );
}



