import * as React from 'react';
import DeleteIcon from '@mui/icons-material/Delete';
import { Box } from '@mui/material';
import axios from "axios";
import swal from 'sweetalert';
import CircularProgress from '@mui/material/CircularProgress';
import LoadingButton from '@mui/lab/LoadingButton';



type subjectsType = {
    subjectId: number;
    subjectName: string;
    parentID: number;
    userId: number;
    color: string;
}
type CustomizedListProps = {
    renderfunc: () => void;
    thisSubjectId: number;
    thisSubjectName: string;
}


// קומפוננטה האחראית על מחיקת נושא קיים -- אייקון פח
export default function RemoveSubject(props: CustomizedListProps) {
    const [loading, setLoading] = React.useState<boolean>(false);

    //שולחת לשרת את הנושא למחיקה
    async function RemoveSubject(subName: string, subId: number) {
        setLoading(true);


        let data = {
            subjectId: subId
        }
        swal({
            title: "!?האם אתה בטוח",
            text: `?"האם אתה מעוניין למחוק את נושא "${subName} `,
            icon: "warning",
            buttons: [true, true],
            dangerMode: true,
        })
            .then(async(willDelete) => {
                if (willDelete) {
                    const response = await axios.post("https://localhost:44391/api/Subject/DeleteSubject", data);
                    let message = response.data;

                    if (message === "Success") {
                        props.renderfunc();
                        swal(`!הנושא "${subName}" נמחק בהצלחה`, {
                            icon: "success",
                        })

                    }
                }
            });
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
                    aria-controls='fade-menu'
                    aria-haspopup="true"
                    aria-expanded='true'
                    onClick={() => RemoveSubject(props.thisSubjectName, props.thisSubjectId)}
                    loading={loading}
                >
                    <DeleteIcon style={{ color: "darkslategrey" }} />
                </LoadingButton>
            </Box>

        </div>

    );
}






