#!/usr/bin/python3
import sys
import argparse
from fpdf import FPDF

def convert_to_pdf(source_files, image_files, output_file):
    pdf = FPDF()
    pdf.add_page()

    # Set font for code
    pdf.set_font("Courier", size=10)

    # Write source code to PDF
    for source_file in source_files:
        with open(source_file, "r") as f:
            code = f.readlines()
        for line in code:
            pdf.cell(200, 10, txt=line, ln=True)

    # Add images to PDF
    for image_file in image_files:
        pdf.image(image_file, x=10, y=pdf.get_y()+10, w=100)

    # Save the PDF to a file
    pdf.output(output_file)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Export source code and images to PDF")
    parser.add_argument("--source_files", nargs="+", help="Source code files to include in the PDF")
    parser.add_argument("--image_files", nargs="+", help="Image files to include in the PDF")
    parser.add_argument("--output", required=True, help="Output PDF file name")
    args = parser.parse_args()

    if not args.source_files and not args.image_files:
        print("Error: No source files or image files provided.")
        sys.exit(1)

    convert_to_pdf(args.source_files, args.image_files, args.output)
