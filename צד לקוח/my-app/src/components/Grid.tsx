import * as React from 'react';
import { styled } from '@mui/material/styles';
import { Box } from '@mui/material';
import Paper from '@mui/material/Paper';
import EmailBox from './EmailBox';
import Navbar from './Navbar';
import { useState } from 'react'
import Autocomplete from '@mui/material/Autocomplete';
import axios from "axios";
import { connect } from 'react-redux';
import SubjectBox from './SubjectBox';
import logo from "../logo.png";
import Aboat from './About';
import { useNavigate } from 'react-router-dom';
import { login, logout } from '../store/actions/user'


// קומפוננטה האחראית על הפריסה של העמוד של האתר היא קוראת לכל שאר הקומפוננטות
function CSSGrid(props: any) {

    const [filter, setFilter] = useState("$all");

    let emails: {
        toUserId: number;
        color: string;
        id: number;
        email: string;
        name: string;
        date: string;
        short: string;
        subject: string;
        userSubjets: any;
    }[] = [];
    let tempEmails = emails;


    let emailsFromServer: {
        IdInOutLook: string;
        body: string;
        dateReceived: any;
        fromAddressMail: string;
        fromName: string;
        RelatedSentence: string;
        mailID: number;
        subjectMail: number;
        title: string;
        toUserId: number;
    }[] = [];

    let subjects: {
        subjectId: number
        subjectName: string
        parentID: number
        userId: number
        color: string
    }[] = [];
    let tempSubjects = subjects;

    let setSubjects: (arg0: any) => void;
    [subjects, setSubjects] = useState([]);

    let setEmails: (arg0: any) => void;
    [emails, setEmails] = useState([]);

    // שינוי הפילטר- קטגורית המיילים לתצוגה בלעדית
    function changeFilter(newFilter: string) {
        setFilter(newFilter);
    }

    const [render, setRender] = useState<boolean>(false);
    const navigate = useNavigate();
    let mail = props.email;
    let password = props.password;
    if (mail == null) {
        mail = localStorage.getItem('mail');
        password = localStorage.getItem('password');
        if (mail != null && password != null)
            props.in({ email: mail, password: password + "" });
    }

    React.useEffect(() => {
        const getEmails = async () => {

            const user = { mail: mail, userPassword: password }

            const response1 = await axios.post(`https://localhost:44391/api/Email/GetEmails`, user);
            const response2 = await axios.post("https://localhost:44391/api/Subject/GetSubjects", user);

            emailsFromServer = response1.data;
            emailsFromServer = emailsFromServer.reverse();
            tempSubjects = response2.data;

            if (emailsFromServer != null) {
                emailsFromServer.forEach(x => {
                    tempEmails.push({ userSubjets: [], toUserId: x.toUserId, color: "", id: x.mailID, email: x.fromAddressMail, name: x.fromName, date: x.dateReceived.slice(0, 10), short: x.RelatedSentence, subject: "" });
                })

                emailsFromServer.forEach((x, i) => {
                    let sub = tempSubjects.find(s => s.subjectId === x.subjectMail);
                    if (sub != null) {
                        tempEmails[i].subject = sub.subjectName;
                        tempEmails[i].color = sub.color;
                    }
                    tempEmails[i].userSubjets = tempSubjects;
                });
            }
            setSubjects({ ...tempSubjects })
            setEmails({ ...tempEmails });

        }
        getEmails();
    }, [render])


    function setRenderFunc() {
        setRender(!render);
    }
    return (<>{emails &&
        <Box sx={{
            width: 1,
            textAlign: 'center',
            '& ::selection': {
                background: "#00000000"
            },
            '& .css-1tlc0yb': {
                backgroundColor: '#fff',
                padding: "5px",
                gap: "0px"
            },
            '& .header': {
                height: '14vh',
                color: "#ff735c",
            },
            '& .footer': {
                height: '4vh',
                color: "grey",
                fontSize: '13px'
            },
            '& .main': {
                height: '74.5vh',
                backgroundColor: "white",
                padding: "10px"
            },
            '& .love': {
                color: '#ff735c'
            },
            '& .css-vc9oue': {
                height: '77.2vh'
            }
        }}>

            <Box display="grid" gridTemplateColumns="repeat(12, 1fr)" gap={1}>
                <Box gridColumn="span 12" >
                    <img style={{ float: "left", padding: "24px 0px 0px 42px", cursor: "pointer" }} onClick={() => { navigate('/'); }} src={logo} height={80} dir="rtl" alt="Logo" />
                </Box>
                {filter == '$all' || !filter.startsWith('$') ? <Box gridColumn="span 10">
                    <div className="main"><EmailBox renderfunc={setRenderFunc} cangeFilter={changeFilter} filter={filter} emailsFromServer={emails} ></EmailBox></div>
                </Box> : filter == '$viewSubject' ?
                    <Box gridColumn="span 10">
                        <div className="main"><SubjectBox cangeFilter={changeFilter} renderfunc={setRenderFunc} subjectsFromServer={subjects} ></SubjectBox></div>
                    </Box> :
                    <Box gridColumn="span 10" sx={{ textAlign: "right", padding: "0 70px" }}>
                        <Aboat ></Aboat>
                    </Box>}
                <Box gridColumn="span 2">
                    <Navbar filter={filter} renderfunc={setRenderFunc} cangeFilter={changeFilter} subjects={subjects}></Navbar>
                </Box>
                <Box gridColumn="span 12" >
                    <div className="footer">made with <span className="love">❤</span> by shoshy bier </div>
                </Box>
            </Box>

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
const mapDispatchToProps = {
    in: login, // הפעולה מאקשין
    out: logout
};
// הקומפוננטה הזו אמורה לקבל ערכים מהסטייט
// connect זה הפרמטר הראשון שמקבלת הפונקציה

// mapStateToProps - מקבלת ערכים מהסטייט הכללי
// mapDispatchToProps - משגרת ערכים לסטייט הכללי - הפונקציות שמשנות - actions
export default connect(mapStateToProps, mapDispatchToProps)(CSSGrid);