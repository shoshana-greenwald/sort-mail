import * as React from 'react';
import { connect } from 'react-redux';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import AddIcon from '@mui/icons-material/Add';
import IconButton from '@mui/material/IconButton';
import { Box } from '@mui/material';
import Tooltip from '@mui/material/Tooltip';
import axios from "axios";
import { LinearProgress } from '@mui/material';
import SubjectComboBox from './SubjectComboBox';
import swal from 'sweetalert';

type subject = {
    subjectId: number;
    subjectName: string;
    parentName: (string | undefined);
    userId: number;
    color: string;
}

type propsType = {
    email: string;
    password: string;
    subjects: subject[];
    renderfunc: () => void;
}


//קומפוננטה האחראית על הוספת נושא
function AddNewSubject(props: propsType) {
    const email = props.email;
    const [color, setColor] = React.useState<string>("#838181");
    const [subject, setSubject] = React.useState<string>("");
    const [parent, setParent] = React.useState<string>("");
    const [thinking, setThinking] = React.useState<boolean>(false);

    //פונקציה הבודקת האם נושא מסוים כבר קיים ברשימת הנושאים
    function CheckIfSubjectExist(subs: subject[], sub: string): boolean {
        for (let index = 0; index < subs.length; index++) {
            if (subs[index].subjectName == sub) {
                swal("🙄 נושא זה כבר קיים במערכת");
                return false;
            }
        }
        return true;
    }

    //פונקציה הבודקת האם שם הנושא מכיל רק אותיות עבריות
    function CheckValidation(sub: string): boolean {
        const whietlist = "אבגדהוזחטיכךלמםנןסעפףצץקרשת ";
        for (let i = 0; i < subject.length; i++) {
            if (!whietlist.includes(subject.charAt(i))) {
                swal("🙄 שם הנושא אינו חוקי, שם נושא יכול לכלול אותיות א - ת");
                return false;
            }
        }
        return true;

    }

    //פונקציה המקבלת נושא חדש ואם הוא תקין היא שולחת אותו לשרת
    async function CheckSubject() {
        if (subject == "") {
            swal("🙄 שם הנושא אינו חוקי, שם נושא יכול לכלול אותיות א - ת");
            return;
        }
        setThinking(true);
        let ok: boolean = true;
        const subs: subject[] = Object.values(props.subjects);
        if (CheckIfSubjectExist(subs, subject) && CheckValidation(subject)) {
            //אם שם הנושא הוא יותר ממילה אחת
            if (subject.includes(' ')) {
                ok = false;
                await swal("לידיעתך -- מומלץ להגדיר נושא בעל מילה אחת בלבד", " ??האם את/ה מעוניין להמשיך בכל זאת", {
                    buttons: [true, true]
                }).then((confirm) => {
                    if (confirm)
                        ok = true;
                });
            }
            if (ok)
                AddSubject()
            else setThinking(false);

        }
        else setThinking(false);
    }


    //שולחת את המייל החדש לשרת
    async function AddSubject() {
        let data = {
            subjectName: subject,
            parentName: parent,
            userMail: email,
            color: color,
        }
        const response = await axios.post("https://localhost:44391/api/Subject/AddSubject", data);
        let message = response.data;

        if (message === "Success") {
            props.renderfunc();
        }
        setSubject("");
        setParent("");

        setThinking(false);
    }

    return (<>
        <div dir="rtl" style={{ paddingRight: "43px" }}>
            <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={1}>

                <Box gridColumn="span 12" height={43}>
                    <p style={{ textAlign: "right", height: "38px" }}>הוספת נושא חדש:</p>
                </Box>

                <Box gridColumn="span 2" sx={{ marginBottom: "10px" }} >
                    {thinking ? <Box sx={{ width: '65%' }}>
                        <LinearProgress />
                    </Box> : <div style={{ height: "2px" }}></div>}
                </Box>

                <Box gridColumn="span 10" >
                    <div></div>
                </Box>

                <Box gridColumn="span 2" >
                    <Tooltip title="שדה חובה*">
                        <TextField value={subject} autoComplete="off" dir="rtl" id="filled-basic" label="*שם נושא:" variant="filled" onChange={(e) => setSubject(e.target.value)} />
                    </Tooltip>
                </Box>

                <Box gridColumn="span 2" >
                    <SubjectComboBox value={parent} subjects={props.subjects} setParentFunction={setParent}></SubjectComboBox>
                </Box>

                <Box gridColumn="span 1" >
                    <Tooltip title="בחר צבע לתווית">
                        <input style={{ height: '55px', width: '55px', cursor: "pointer", float: "right" }} type="color" value={color} onChange={(e) => setColor(e.target.value)}></input>
                    </Tooltip>
                </Box>

                <Box gridColumn="span 1" >
                    <Tooltip title="הוסף נושא חדש">
                        <IconButton aria-label="delete" size="large" onClick={CheckSubject} disabled={thinking} style={{ float: "right" }}>
                            <AddIcon fontSize="inherit" />
                        </IconButton>
                    </Tooltip>
                </Box>

            </Box>
        </div>
    </>)


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
export default connect(mapStateToProps)(AddNewSubject);